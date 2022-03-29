using UnityEngine;
using System.Collections;

public class MiyaPatterns : MonoBehaviour
{
    [Header("Ref:")] 
    [SerializeField] private Collider2D[] platforms;
    [SerializeField] private BoxCollider2D atk3DetectionBox;

    [SerializeField] private Animator miyaAnim;
    [SerializeField] private Animator atk4BlindAnim;

    [SerializeField] private EnemyAI_Ranged shootingAI;
    [SerializeField] private ArmToPlayerTracking trackingAI;
    
    [SerializeField] private LayerMask playerHitLayer;

    private PlayerHpSystem playerHpSystem;
    
    private PlayerMovement playerMovement;
    private Vector2 playerPos;

    private Rigidbody2D rb;
    private Enemy_Flipped enemyFlipped;
    private Collider2D col;

    
    
    [Header("General")]
    [SerializeField] private int attackCounter = 0; //remove later
    [SerializeField]private float[] attackDuration;     
    [SerializeField] private float stopTime = 0f;     //remove later  
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] [Range(0f, 10f)] private float xDistToStartAttack = 1f;
    [SerializeField] [Range(0f, 10f)] private float yDistToStartJumping = 1f;
    [SerializeField] private float timeBetweenJumps = 3f;
    
    private float nextJumpTime = 0f;
    

    [Header("Attack 2")] 
    [SerializeField] private int damageToPlayer = 1;
    [SerializeField] private float atk2AreaOffset;
    [SerializeField] private float atk2AreaSize;

    [Header("Attack 3")]
    [SerializeField] private float dashSpeed = .2f;
    [SerializeField] private float timeBetweenDashes = 3f;
    [SerializeField] private float dashDuration = 1f;
    
    private float nextDashTime = 0f;
    private bool isDoingAtk3 = false;

    [Header("Attack 4")] 
    [SerializeField] private float atk4Radius;
    [SerializeField] private Vector2 atk4Offset;
    
    
    void OnDestroy()
    {
        MiyaAtk3Detection.OnPlayerEnter -= MiyaAtk3_OnPlayerEnter;
    }
    
    void Start()
    {
        Transform playerBody = GameObject.FindGameObjectWithTag("PlayerBody").GetComponent<Transform>();
        playerMovement = playerBody.GetComponent<PlayerMovement>();
        playerHpSystem = playerBody.GetComponent<PlayerHpSystem>();
        
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        enemyFlipped = GetComponent<Enemy_Flipped>();

        MiyaAtk3Detection.OnPlayerEnter += MiyaAtk3_OnPlayerEnter;

        atk3DetectionBox.enabled = false;
        atk4BlindAnim.gameObject.SetActive(false);
        trackingAI.enabled = false;
        shootingAI.enabled = false;
 
        //Start Attack1
        stopTime = Time.time + attackDuration[0];
        ChangeAttack();
    }
    
    void FixedUpdate()
    {
        SetIgnorePlatformCollisions();
        
        playerPos = playerMovement.GetPlayerPos();
        //enemyFlipped.LookAtPlayer();
 
        
        //if hp == 50%, 75%, 20%, trigger atk4
        
        
        
        switch (attackCounter)
        {
            case 1: 
                Attack1();
                break;
            case 2:
                Attack2();
                break;
            case 3:
                Attack3();
                break;
            case 4:
                Attack4();
                break;
        }
    }
    
    //shoot Assault Rifle
    private void Attack1()
    {
        print("pewpew");
        

        if (IsAttackDurationEnded())
        {
            ChangeAttack();
        }
    }
    
    //melee
    private void Attack2()
    {
        if (!ReachedTarget(playerPos))
        {
             MoveToTarget(playerPos);
            
            float diff = playerPos.y - transform.position.y ;
            if (diff > yDistToStartJumping)
            {
                if (Time.time > nextJumpTime)
                {
                    Jump();
                    nextJumpTime = Time.time + timeBetweenJumps;
                }
            }
        }
        else
        {
            print("slashslash");
            //play attack2 animation, add anim event to damage player
        }

        
        if (IsAttackDurationEnded())
        {
            ChangeAttack();
        }
    }

    //quick dash, leaving a trail that damages the player if not avoided
    private void Attack3()
    {
        print("SWOOSSH");
        
       if (Time.time > nextDashTime)
       {
           rb.velocity = new Vector2(0f, 0f);
           Dash();
        
           nextDashTime = Time.time + timeBetweenDashes;
       }
       else
       {
         // MoveToTarget(playerPos);
       }
        
        if (IsAttackDurationEnded())
        {
            ChangeAttack();
        }
    }

    //flash bang helmet
    private void Attack4()
    {
        print("bang bang bang");
        //play blind telegraph animation

        miyaAnim.Play("miyaBlind");
        ChangeAttack();
    }

    private void CalcAttackEndTime()
    {
        if (Time.time > stopTime)
        {
            stopTime = Time.time + attackDuration[attackCounter - 1];
        }
    }
    
    private void ChangeAttack()
    {
        attackCounter++;
        if (attackCounter > 4)
        {
            attackCounter = 1;
        }

        if (attackCounter == 1)
        {
            trackingAI.enabled = true;
            Invoke(nameof(EnableShootingAI), 1f);
        }
        else
        {
            if (enemyFlipped.GetIsFacingRight())
            {
                trackingAI.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
            else
            {
                trackingAI.transform.eulerAngles = new Vector3(0f, 0f, 0);
            }
           // trackingAI.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            
            trackingAI.enabled = false;
            shootingAI.enabled = false;
        }
        
        CalcAttackEndTime();
    }

    private void EnableShootingAI()
    {
        shootingAI.enabled = true;
    }

    private bool IsAttackDurationEnded()
    {
        return Time.time > stopTime;
    }

    private void MoveToTarget(Vector2 targetPos)
    {
        enemyFlipped.LookAtPlayer();
        float targetDir = targetPos.x - transform.position.x;


        float speedX = moveSpeed;
        if (targetDir < 0f)
        {
            speedX *= -1;
        }
 
        rb.AddForce(new Vector2(speedX, rb.velocity.y) * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        print("jumping");
        rb.AddForce(new Vector2(rb.velocity.x, jumpForce) * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }
    
    private bool ReachedTarget(Vector2 targetPos)
    {
        if (Vector2.Distance(targetPos, transform.position) > xDistToStartAttack)
        {
            return false;
        }
        
        return true;
    }

    private void SetIgnorePlatformCollisions()
    {
        if (rb.velocity.y > 0f)
        {
            IgnorePlatformCollisions(true);
        }
        else
        {
            IgnorePlatformCollisions(false);
        }
    }

    //for when miya is jumping, to avoid collision with the floating platforms
    private void IgnorePlatformCollisions(bool status)
    {
        foreach (Collider2D platform in platforms)
        {
            Physics2D.IgnoreCollision(platform, col, status);
        }
    }

    public void Attack2DamagePlayer()
    {
        Collider2D hitPlayer;

        if (!enemyFlipped.GetIsFacingRight())
        {
            hitPlayer = Physics2D.OverlapCircle(transform.position - new Vector3(atk2AreaOffset, 0f, 0f ), atk2AreaSize, playerHitLayer);
        }
        else
        {
            hitPlayer = Physics2D.OverlapCircle(transform.position - new Vector3(-atk2AreaOffset, 0f, 0f ), atk2AreaSize, playerHitLayer);
        }
        
        if (hitPlayer != null)
        {
            playerHpSystem.TakeDamage(damageToPlayer);
        }
    }

    private void Dash()
    {
        isDoingAtk3 = false;
        atk3DetectionBox.enabled = true;
        
        float dirX = 1f;
        if (!enemyFlipped.GetIsFacingRight())
        {
            dirX *= -1;
        }

        rb.velocity = new Vector2(dirX * dashSpeed, rb.velocity.y);
        StartCoroutine(DashStop());
    }

    private IEnumerator DashStop()
    {
        yield return new WaitForSeconds(dashDuration);
        
        rb.velocity = new Vector2(0f, 0f);
        atk3DetectionBox.enabled = false;
    }

    private void Atk3Slash()
    {
        if (isDoingAtk3) return;
        isDoingAtk3 = true;
        
        print("attak");
    }
    
    private void MiyaAtk3_OnPlayerEnter()
    {
        Atk3Slash();
    }

    public void BlindPlayer()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle((Vector2)transform.position + atk4Offset, atk4Radius, playerHitLayer);

        if (hitPlayer != null)
        {
            if (!atk4BlindAnim.gameObject.activeSelf)
            {
                atk4BlindAnim.gameObject.SetActive(true);
            }
            
           
            atk4BlindAnim.Play("blindEffect_FadeIn");
            print("banged");
        }
        miyaAnim.Play("miyaIdle");
    }
    
    // private void OnDrawGizmos()
    // {
    //     //atk2
    //     Gizmos.DrawSphere(transform.position - new Vector3(atk2AreaOffset, 0f, 0f ), atk2AreaSize);
    //     Gizmos.DrawSphere(transform.position - new Vector3(-atk2AreaOffset, 0f, 0f ), atk2AreaSize);
    //     
    //     //atk4
    //     Gizmos.DrawSphere((Vector2)transform.position + atk4Offset, atk4Radius);
    // }
}
    