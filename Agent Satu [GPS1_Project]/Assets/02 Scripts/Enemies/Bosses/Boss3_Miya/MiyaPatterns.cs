using UnityEngine;
using System.Collections;

public class MiyaPatterns : MonoBehaviour
{
    [Header("Ref:")] 
    [SerializeField] private Collider2D[] platforms;
    [SerializeField] private LayerMask playerHitLayer;

    private PlayerHpSystem playerHpSystem;
    
    
    [Header("General")]
    [SerializeField] private int attackCounter = 0; //remove later
    [SerializeField]private float[] attackDuration;     
    [SerializeField] private float stopTime = 0f;     //remove later  
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] [Range(0f, 10f)] private float yDistToStartJumping = 1f;
    [SerializeField] private float timeBetweenJumps = 3f;
    
    private float nextJumpTime = 0f;


    [SerializeField] [Range(0f, 10f)] private float xDistToStartAttack = 1f;

    private PlayerMovement playerMovement;
    private Vector2 playerPos;

    private Rigidbody2D rb;
    private Enemy_Flipped enemyFlipped;
    private Collider2D col;

    [Header("Attack 2")] 
    [SerializeField] private int damageToPlayer = 1;
    [SerializeField] private float attack2AreaOffset;
    [SerializeField] private float attack2AreaSize;

    [Header("Attack 3")] 
    [SerializeField] private float dashDist = 1f;
    [SerializeField] private float dashSpeed = .2f;
    [SerializeField] private float timeBetweenDashes = 3f;
    [SerializeField] private float dashDuration = 1f;
    private float nextDashTime = 0f;
    
 


    
    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("PlayerBody").GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        enemyFlipped = GetComponent<Enemy_Flipped>();
        
        //Start Attack1
        stopTime = Time.time + attackDuration[0];
        ChangeAttack();
    }
    
    void FixedUpdate()
    {
        enemyFlipped.LookAtPlayer();
        SetIgnorePlatformCollisions();
        
        playerPos = playerMovement.GetPlayerPos();
        
        switch (3)
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
        
       // Physics.OverlapCapsule()

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
        
        if (IsAttackDurationEnded())
        {
            ChangeAttack();
        }
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
        CalcAttackEndTime();
    }

    private bool IsAttackDurationEnded()
    {
        return Time.time > stopTime;
    }

    private void MoveToTarget(Vector2 targetPos)
    {
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
        foreach (Collider2D c in platforms)
        {
            Physics2D.IgnoreCollision(c, col, status);
        }
    }

    public void Attack2DamagePlayer()
    {
        Collider2D hitPlayer;

        if (!enemyFlipped.GetIsFacingRight())
        {
            hitPlayer = Physics2D.OverlapCircle(transform.position - new Vector3(attack2AreaOffset, 0f, 0f ), attack2AreaSize, playerHitLayer);
        }
        else
        {
            hitPlayer = Physics2D.OverlapCircle(transform.position - new Vector3(-attack2AreaOffset, 0f, 0f ), attack2AreaSize, playerHitLayer);
        }
        
        if (hitPlayer != null)
        {
            Transform playerRoot = hitPlayer.transform.root;
            
            //If no ref already
            if (playerHpSystem == null)
            {
                //Locate PlayerBody to get hp system script
                playerHpSystem = playerRoot.GetChild(0).GetComponent<PlayerHpSystem>();
            }
            playerHpSystem.TakeDamage(damageToPlayer);
        }
    }

    private void Dash()
    {
        float dirX = 1f;
        if (!enemyFlipped.GetIsFacingRight())
        {
            dirX *= -1;
        }

       
        StartCoroutine(DashStop());
        rb.velocity = new Vector2(dirX * dashSpeed, rb.velocity.y);
    }

    private IEnumerator DashStop()
    {
        //yield return new WaitForSeconds(dashDuration);

        float destX = (transform.position.x + dashDist) * -transform.right.x;
        float startX = transform.position.x;
        while (transform.position.x < destX)
        {
            yield return null;
        }
        rb.velocity = new Vector2(0f, 0f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position - new Vector3(attack2AreaOffset, 0f, 0f ), attack2AreaSize);
        Gizmos.DrawSphere(transform.position - new Vector3(-attack2AreaOffset, 0f, 0f ), attack2AreaSize);
    }
}
