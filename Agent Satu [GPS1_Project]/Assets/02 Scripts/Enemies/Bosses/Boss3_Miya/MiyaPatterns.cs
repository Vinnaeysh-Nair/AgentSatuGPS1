using UnityEngine;
using System.Collections;

public class MiyaPatterns : MonoBehaviour
{
    [Header("Ref:")] 
    [SerializeField] private Collider2D[] platforms;
    [SerializeField] private Animator miyaAnim;
    [SerializeField] private Transform bodyCenter;
  
    [SerializeField] private ArmToPlayerTracking trackingAI;
    [SerializeField] private LayerMask playerHitLayer;

    private PlayerHpSystem playerHpSystem;
    private PlayerMovement playerMovement;
    private Vector2 playerPos;

    [SerializeField] private Rigidbody2D rb;
    private Enemy_Flipped enemyFlipped;
    private EnemyAI_Ranged shootingAI;
    private Enemy_Agro enemyAgro;
    private Collider2D col;

    
    
    [Header("General")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] [Range(0f, 10f)] private float xDistToStartAttack = 1f;
    [SerializeField] [Range(0f, 10f)] private float yDistToStartJumping = 1f;
    [SerializeField] private float timeBetweenJumps = 3f;
    
    private int attackCounter = 0; 
    private float stopTime = 0f;     
    
    private float nextJumpTime = 0f;

    [Header("Attack 1")]
    [SerializeField] private float standStillDuration = 5f;
    [SerializeField] private Transform[] atk1movementPoints;

    private int nextMovementPoint = 0;
    private bool isStandingStill = false;

    [Header("Attack 2")] 
    [SerializeField] private float atk2Duration = 5f;
    [SerializeField] private int atk2Dmg = 1;
    [SerializeField] private float timeBtwSlashes = .2f;
    [SerializeField] private Vector2 atk2AreaOffset;
    [SerializeField] private float extraOffsetX;
    [SerializeField] private float atk2AreaSize;

    private float _nextAtk2SlashTime = 0f;


    [Header("Attack 3")] 
    [SerializeField] private BoxCollider2D atk3DetectionBox;
    [SerializeField] private TrailRenderer bladeTrail;
    [SerializeField] private int atk3Dmg = 10;
    [SerializeField] private float dashSpeed = .2f;
    [SerializeField] private float timeBetweenDashes = 3f;
    [SerializeField] private float dashDuration = 1f;
    [SerializeField] private float atk3Duration = 5f;
    [SerializeField] private float atk3DamageDelay = 3f;
    
    private Vector2 dashStartPos;
    private Vector2 dashEndPos;
    private float dashDistanceTravelled;


    private float atk3TimeToTakeDamage = 3f;

    [Header("Attack 4")] 
    [SerializeField] private Animator atk4FlashStart;
    [SerializeField] private Animator atk4BlindAnim;
    
    private float nextDashTime = 0f;
    private bool isDoingAtk3 = false;

    void OnDestroy()
    {
        MiyaAtk3Detection.OnPlayerEnter -= MiyaAtk3_OnPlayerEnter;
        MiyaHp.OnReachingThreshold -= MiyaHp_OnReachingThreshold;
    }
    
    void Start()
    {
        Transform playerBody = GameObject.FindGameObjectWithTag("PlayerBody").GetComponent<Transform>();
        playerMovement = playerBody.GetComponent<PlayerMovement>();
        playerHpSystem = playerBody.GetComponent<PlayerHpSystem>();
        
        col = GetComponent<Collider2D>();
        enemyFlipped = GetComponent<Enemy_Flipped>();
        enemyAgro = GetComponent<Enemy_Agro>();
        shootingAI = GetComponent<EnemyAI_Ranged>();
 
        MiyaAtk3Detection.OnPlayerEnter += MiyaAtk3_OnPlayerEnter;
        MiyaHp.OnReachingThreshold += MiyaHp_OnReachingThreshold;

        atk3DetectionBox.enabled = false;
        atk4BlindAnim.gameObject.SetActive(false);
        atk4FlashStart.gameObject.SetActive(false);
        trackingAI.enabled = false;
        shootingAI.enabled = false;
        bladeTrail.gameObject.SetActive(false);
        
        //Start Attack1
        ChangeAttack();
    }
    
    void FixedUpdate()
    {
        SetIgnorePlatformCollisions();
        
        playerPos = playerMovement.GetPlayerPos();
     
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
        }

        //if grounded
        if (rb.velocity.y < .01f)
        {
            miyaAnim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        }
    }
    
    //shoot Assault Rifle
    private void Attack1()
    {
        Vector2 targetPos = atk1movementPoints[nextMovementPoint].position;
        if (!ReachedTarget(targetPos, false))
        {
            if (isStandingStill) return;
            
            MoveToTarget(targetPos);
        }
        else
        {
            //print("reached");
            rb.velocity = new Vector2(0f, 0f);
            nextMovementPoint++;
            
            if (nextMovementPoint > atk1movementPoints.Length - 1)
            {
                nextMovementPoint = 0;
                ChangeAttack();
                return;
            }
            
            isStandingStill = true;
            Invoke(nameof(SetIsStandingStillToFalse), standStillDuration);
        }
    }
    
    //melee
    private void Attack2()
    {
        if (!ReachedTarget(playerPos, true))
        {
            MoveToTarget(playerPos);
            
  
            miyaAnim.SetBool("IsAttacking", false);
            bladeTrail.gameObject.SetActive(false);
        }
        else
        {
            if (Time.time > _nextAtk2SlashTime)
            {
                bladeTrail.Clear();
                bladeTrail.gameObject.SetActive(true);

                miyaAnim.SetBool("IsAttacking", true);
                _nextAtk2SlashTime = Time.time + timeBtwSlashes;
            }
        }
        
        if (IsAttackDurationEnded())
        {
            miyaAnim.SetBool("IsAttacking", false);
            bladeTrail.gameObject.SetActive(false);
            ChangeAttack();
            
            //buffer time before Atk3 starts
            nextDashTime = Time.time + 3f;
        }
    }
    
    //used in Animator
    public void Attack2DamagePlayer()
    {
        Collider2D hitPlayer;

        Vector2 offset = atk2AreaOffset;
        if (!enemyFlipped.GetIsFacingRight())
        {
            hitPlayer = Physics2D.OverlapCircle(transform.position - new Vector3(offset.x + extraOffsetX, offset.y), atk2AreaSize, playerHitLayer);
        }
        else
        {
            hitPlayer = Physics2D.OverlapCircle(transform.position - new Vector3(-offset.x + extraOffsetX - 2f, offset.y, 0f ), atk2AreaSize, playerHitLayer);
        }
        
        if (hitPlayer != null)
        {
            playerHpSystem.TakeDamage(atk2Dmg);
        }
    }
    
    //used in Animator
    public void SetAtk2IsAttackingToFalse()
    {
        miyaAnim.SetBool("IsAttacking", false);
    }
    
    private void SetIsStandingStillToFalse()
    {
        isStandingStill = false;
    }
    
    private void MoveToTarget(Vector2 targetPos, bool canJump = true)
    {
        float targetDir = targetPos.x - transform.position.x;


        float speedX = moveSpeed;
        if (targetDir < 0f)
        {
            speedX *= -1;
        }

        rb.AddForce(new Vector2(speedX, rb.velocity.y) * Time.fixedDeltaTime);

        if (!canJump) return;
        
        float diff = targetPos.y - transform.position.y ;
        if (diff > yDistToStartJumping)
        {
            if (Time.time > nextJumpTime)
            {
                Jump();
                nextJumpTime = Time.time + timeBetweenJumps;
            }
        }
    }
    
    private bool ReachedTarget(Vector2 targetPos, bool targetIsPlayer)
    {
        float compDist;
        if (targetIsPlayer)
        {
            compDist = xDistToStartAttack + atk2AreaOffset.x;
        }
        else
        {
            compDist = 1f;
        }

        
        if (Vector2.Distance(targetPos, transform.position) > compDist)
        {
            return false;
        }
        
        return true;
    }

    //quick dash, leaving a trail that damages the player if not avoided
    private void Attack3()
    {
        bladeTrail.gameObject.SetActive(true);
        if (isStandingStill) return;
       
        if (Time.time > nextDashTime)
        {

            rb.velocity = new Vector2(0f, rb.velocity.y);
            Dash();
        
            nextDashTime = Time.time + timeBetweenDashes;
        }
        else
        {
            MoveToTarget(playerPos, true);
        }
        
        if (IsAttackDurationEnded())
        {
            ChangeAttack();
        }
    }
    
    private void Dash()
    {
        dashStartPos = bodyCenter.position;
        
        isDoingAtk3 = false;
        atk3DetectionBox.enabled = true;
        
        enemyAgro.enabled = false;
        isStandingStill = true;


        Invoke(nameof(SetIsStandingStillToFalse), 2f);
        
        
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
        
        dashEndPos = bodyCenter.position;
        dashDistanceTravelled = Vector2.Distance(dashEndPos, dashStartPos);
        atk3DetectionBox.size = new Vector2(dashDistanceTravelled - 3f, atk3DetectionBox.size.y);
       
        
        StartCoroutine(DisableBladeTrail());
    }

    private IEnumerator DisableBladeTrail()
    {
        yield return new WaitForSeconds(3f);
        bladeTrail.Clear();

        bladeTrail.gameObject.SetActive(false);
        enemyAgro.enabled = true;
        atk3DetectionBox.enabled = false;
    }

    private void Atk3Slash()
    {
        if (!isDoingAtk3)
        {
            atk3TimeToTakeDamage = Time.time + atk3DamageDelay;
            isDoingAtk3 = true;
        }

        if (isDoingAtk3)
        {
            if (Time.time > atk3TimeToTakeDamage)
            {
                playerHpSystem.TakeDamage(atk3Dmg);
                isDoingAtk3 = false;
            }
        }
    }
    
    private void MiyaAtk3_OnPlayerEnter()
    {
        Atk3Slash();
    }


    //flash bang helmet
    private void Attack4()
    {
        attackCounter = 4;
  
        //play blind telegraph animation
        atk4FlashStart.gameObject.SetActive(true);
        
        ChangeAttack();
    }

    private void ChangeAttack()
    {
        attackCounter++;
        UpdateAttackEndTime();
        
        
        //Loop back
        if (attackCounter > 3)
        {
            attackCounter = 1;
        }
        
        
        //Adjustment for Atk1
        if (attackCounter == 1)
        {
            trackingAI.enabled = true;
            Invoke(nameof(EnableShootingAI), 1f);
        }
        else
        {
            //places the gun in its original position
            if (enemyFlipped.GetIsFacingRight())
            {
                trackingAI.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
            else
            {
                trackingAI.transform.eulerAngles = new Vector3(0f, 0f, 0);
            }
  
            trackingAI.enabled = false;
            shootingAI.enabled = false;
        }
    }
    
    private void CalcAttackEndTime(float duration)
    {
        stopTime = 0f;
        if (Time.time > stopTime)
        {
            stopTime = Time.time + duration;
        }
    }

    private bool IsAttackDurationEnded()
    {
        return Time.time > stopTime;
    }

    private void UpdateAttackEndTime()
    {
        switch (attackCounter)
        {
            case 2:
                CalcAttackEndTime(atk2Duration);
                break;
            case 3:
                CalcAttackEndTime(atk3Duration);
                break;
        }
    }
    
    private void Jump()
    {
        miyaAnim.SetTrigger("IsJumping");
        rb.AddForce(new Vector2(rb.velocity.x, jumpForce) * Time.fixedDeltaTime, ForceMode2D.Impulse);
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

   
  
    private void MiyaHp_OnReachingThreshold()
    {
        Attack4();
    }

    public void BlindPlayer()
    {
        atk4BlindAnim.gameObject.SetActive(true);
        atk4BlindAnim.Play("blindEffect_FadeIn");
        miyaAnim.Play("idle_miya");
    }
    
    private void EnableShootingAI()
    {
        shootingAI.enabled = true;
    }
    
    // private void OnDrawGizmos()
    // {
    //     //atk2
    //     Vector2 offset = atk2AreaOffset;
    //     Gizmos.DrawSphere(transform.position - new Vector3(offset.x + extraOffsetX, offset.y, 0f ), atk2AreaSize);
    //     Gizmos.DrawSphere(transform.position - new Vector3(-offset.x + extraOffsetX - 2f, offset.y, 0f ), atk2AreaSize);
    // }
}
    