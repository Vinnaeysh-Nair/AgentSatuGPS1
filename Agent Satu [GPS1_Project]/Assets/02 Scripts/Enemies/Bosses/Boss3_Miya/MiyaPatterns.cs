using UnityEngine;
using System.Collections;

public class MiyaPatterns : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private int attackCounter = 0;
    [SerializeField]private float[] attackDuration;
    [SerializeField] private float stopTime = 0f;

    [SerializeField] [Range(0f, 10f)] private float xDistToStartAttack = 1f;

    private PlayerMovement playerMovement;
    private Vector2 playerPos;

    private Rigidbody2D rb;

    [Header("Attack 2")] 
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] [Range(0f, 10f)] private float yDistToStartJumping = 1f;

    private bool isJumping = false;
    private bool settingToFalse = false;
    
    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("PlayerBody").GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        
        //Start Attack1
        stopTime = Time.time + attackDuration[0];
        ChangeAttack();
    }
    
    void FixedUpdate()
    {
        playerPos = playerMovement.GetPlayerPos();




        
        switch (2)
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
                if (!isJumping)
                {
                    Jump();
                }
            }
        }
        else
        {
            print("slashslash");
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
        rb.AddForce(new Vector2(rb.velocity.x, jumpForce) * Time.fixedDeltaTime, ForceMode2D.Impulse);
        
        if (!settingToFalse)
        {
            settingToFalse = true;
            StartCoroutine(SetJumpingToFalse());
        }
    }

    private IEnumerator SetJumpingToFalse()
    {
        yield return new WaitForSeconds(.5f);
        isJumping = false;

        settingToFalse = false;
    }

    private bool ReachedTarget(Vector2 targetPos)
    {
        if (Vector2.Distance(targetPos, transform.position) > xDistToStartAttack)
        {
            return false;
        }
        
        return true;
    }
}
