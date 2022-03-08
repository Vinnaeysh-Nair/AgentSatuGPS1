using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Components    
    //public PlayerAnimationController animCon;
    public CrosshairAiming aim;
    [SerializeField] private Rigidbody2D rb;
    
    //Detector components
    private BoxCollider2D upperBodyPlatformDetector;
    private BoxCollider2D lowerBodyPlatformDetector;
    private BoxCollider2D upperBodyHitDetector;
    private BoxCollider2D lowerBodyHitDetector;

    
    //Fields
    //General movement fields
    [Header("General Movement")]
    [SerializeField] private float horizontalMoveSpeed = 20f;
    [SerializeField] private float jumpForce = 30f;
    [SerializeField] private float gravity = 9f;
    [SerializeField] [Range(0f, 1f)] private float horizontalLerp = .3f;
    [SerializeField] [Range(0f, 1f)] private float verticalLerp = .8f;
    [SerializeField] [Range(0f, 1f)] private float crouchSlowMultiplier;
    private bool grounded = true;
    private bool facingRight = true;
    private bool wasCrouching = false;
    private bool playerIsCrouching = false;

    //Dodgeroll fields
    [Header("Dodgeroll")]
    [SerializeField] private float dodgerollHorizontal = 50f;
    [SerializeField] private float dodgerollVertical = 10f;
    [SerializeField] private float cooldownTime = 2f;
    [SerializeField] private float immunityTime = .5f;
    private float nextDodgerollTime = 0f;


    //Walljump fields
    [Header("Wall Jump")]
    [SerializeField] private float xWallJumpForce;
    [SerializeField] private float yWallJumpForce;
    [SerializeField] private float leftOrRightJumpCooldownTime = .3f;
    [SerializeField] [Range(0f, 1f)] private float wallJumpGravityReduction = .2f;
    private bool canWallJump = false;
    private bool canWallJumpLeft = false;
    private bool canWallJumpRight = false;
    private float wallJumpDelay = .08f;
    private bool jumpedToLeft = false;
    private bool jumpedToRight = false;
    

    
    //Groundcheck and Ceiling check fields
    [SerializeField] private LayerMask platformLayerMask;
    private Collider2D ceilingCheck;

   private bool droppedFromStair = false;
    
    //Getter
    public bool GetGrounded()
    {
        return grounded;
    }

    public bool GetPlayerIsCrouching()
    {
        return playerIsCrouching;
    }
    
    void Awake()    
    {
        //rb = GetComponent<Rigidbody2D>();
        
        //Set to customized gravity
        rb.gravityScale = gravity;
        
        //Find Ground collider
        BoxCollider2D[] platformDetectors = transform.Find("Detectors/PlatformDetectors").GetComponentsInChildren<BoxCollider2D>();
        upperBodyPlatformDetector = platformDetectors[0];
        lowerBodyPlatformDetector = platformDetectors[1];


        //Find Hit colliders, these colliders should be triggers
        BoxCollider2D[] hitDetectors = transform.Find("Detectors/HitDetectors").GetComponentsInChildren<BoxCollider2D>();
        upperBodyHitDetector = hitDetectors[0];
        lowerBodyHitDetector = hitDetectors[1];
    }

    //To detect if touching anything on platformLayerMask
    void Update()
    {
        RaycastHit2D collidedPlatform = TouchingPlatformCheck();
        
        //Nothing detected
        if (collidedPlatform.collider == null) return;
        
        
        //When touching anything other than ground
        canWallJumpLeft = (collidedPlatform.normal == Vector2.left);
        canWallJumpRight = (collidedPlatform.normal == Vector2.right);
        
        if (canWallJumpLeft|| canWallJumpRight)
        {
            canWallJump = true;
            return;
        }
        
        //When touching ground
        grounded = true;
    }
    
    
    //For when player is on Stairs (prevents sliding off when not moving)
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Stair"))
        {
            if (playerIsCrouching && !droppedFromStair)
            {
                droppedFromStair = true;
                StartCoroutine(SetDroppedFromStairToFalse());
            }
            
            
            if (droppedFromStair)
            {
                rb.gravityScale = gravity;
                return;
            }
            rb.gravityScale = 0f;
        }
    }
    
    //If not on stairs anymore, set gravity again (player going too fast into stairs will end up hopping a bit, causing them to leave the stairs)
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Stair"))
        {
            rb.gravityScale = gravity;
        }
    }
    
    public void Move(float horizontalMoveDir, bool jump, bool crouch, bool dodgeroll)
    {
        float horizontalMove = rb.velocity.x;
        float verticalMove = rb.velocity.y;

        
        //Jump
        if (jump && grounded)
        {
            grounded = false;
            verticalMove = jumpForce;
            
            //animCon.OnJumping();
        } 
        else if (jump && canWallJump)
        {
            Vector2 wallJumpMovement = WallJump();
            
            horizontalMove = xWallJumpForce * wallJumpMovement.x;
            verticalMove = yWallJumpForce * wallJumpMovement.y;
        }
        //Crouch
        else if (crouch && grounded)
        {
            horizontalMove = Crouch(horizontalMoveDir);
        }
        //Dodgeroll
        else if (dodgeroll && grounded)
        {
            Vector2 dodgerollMovement = Dodgeroll();

            horizontalMove = dodgerollMovement.x;
            verticalMove = dodgerollMovement.y;
        }
        else
        {
            //If ceiling above, keep crouching (ignore stairs)
            if (ceilingCheck != null && !ceilingCheck.CompareTag("Stair"))
            {
                horizontalMove = Crouch(horizontalMoveDir);
            }
            //Default state
            else
            {
                horizontalMove = DefaultMove(horizontalMoveDir);
            }
        }
        
        
        //Apply velocity
        horizontalMove = Mathf.Lerp(rb.velocity.x, horizontalMove, horizontalLerp);
        verticalMove = Mathf.Lerp(rb.velocity.y, verticalMove, verticalLerp);
        
        rb.velocity = new Vector2(horizontalMove, verticalMove);
 
        
        
        //Flip player according to mouse position
        Vector2 mousePos = aim.GetMousePos();
        float mouseDir = mousePos.x - transform.position.x;
                    
        //If player facing right & mouse is to the left, flip player to left
        if (mouseDir > 0 && !facingRight)
        {
            Flip();
        }
        //If player facing left & mouse is to the right, flip player to right
        else if (mouseDir <= 0 && facingRight)
        {
            Flip();
        }
    }


    //Movement functions
    private float DefaultMove(float horizontalMoveDir)
    {
        playerIsCrouching = false;
        
        
        float defaultSpeed = horizontalMoveDir * horizontalMoveSpeed;
                
        //After crouching, re-enable upper body collider
        if (wasCrouching)
        {
            EnableUpperBodyPlatformDetector(true);
            EnableUpperBodyHitDetector(true);
                
            wasCrouching = false;
        }
            
        //animCon.OnCrouchReleasing();
        //animCon.OnRunning();

        return defaultSpeed;
    }

    private float Crouch(float horizontalMoveDir)
    {
        playerIsCrouching = true;
        
        float crouchMoveSpeed = horizontalMoveDir * horizontalMoveSpeed * (1f - crouchSlowMultiplier); 
        wasCrouching = true;


        //Disable upper body collision
        EnableUpperBodyPlatformDetector(false);
        EnableUpperBodyHitDetector(false);
            
        ceilingCheck = Physics2D.OverlapBox(new Vector2(lowerBodyPlatformDetector.bounds.center.x, lowerBodyPlatformDetector.bounds.center.y + (2 * lowerBodyPlatformDetector.bounds.extents.y) + .005f), new Vector2(.5f, lowerBodyPlatformDetector.bounds.size.y), 0f, platformLayerMask);
            
  
        if (horizontalMoveDir > 0f || horizontalMoveDir < 0f)
        {
            //animCon.OnCrouching();
        }
        else
        {
            //crouch walking animation
        }

        return crouchMoveSpeed;
    }

    private Vector2 Dodgeroll()
    {
        float hMove = 0f, vMove = 0f;
        
        //Apply cooldown to dodgeroll
        if (Time.time > nextDodgerollTime)
        {
            //Disable collisions
            EnableUpperBodyHitDetector(false);
            EnableLowerBodyHitDetector(false);
                
            //Enable back after a duration
            StartCoroutine(DisableDodgerollImmuneDamage());
                
            //Direction is according to player's horizontal movement
            if (hMove < 0f || !facingRight)
            {
                hMove = -dodgerollHorizontal;
            }
            else if (hMove >= 0f || facingRight)
            {
                hMove = dodgerollHorizontal;
            }
            vMove = dodgerollVertical;
                            
            //Add animation
                        
            nextDodgerollTime = Time.time + cooldownTime;
        }


        return new Vector2(hMove, vMove);
    }

    private Vector2 WallJump()
    {
        //Reduce gravity, reset after a timer
        rb.gravityScale *= (1 - wallJumpGravityReduction);
        StartCoroutine(ResetGravityScale());
            

        //Wall jump direction according to side detected
        int wallJumpDir = 0;
        int vMoveDir = 1;
            
        if (canWallJumpLeft && !jumpedToLeft)
        {
            canWallJumpLeft = false;
                
            wallJumpDir = -1;
            StartCoroutine(ResetWallJump());
                
            jumpedToLeft = true;
        }
        else if (canWallJumpRight && !jumpedToRight)
        {
            canWallJumpRight = false;

            wallJumpDir = 1;
            StartCoroutine(ResetWallJump());
                
            jumpedToRight = true;
        }
        else
        {
            //If isn't able to wall jump, set x and y velocity to 0
            wallJumpDir = 0;
            vMoveDir = 0;
        }
        
        //Reset player ability to walljump
        StartCoroutine(SetCanWallJumpToFalse());
        
        return new Vector2(wallJumpDir, vMoveDir);
    }
    
    //Other functions
    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180f, 0);
    }

    //For Hit detectors
    private void EnableUpperBodyHitDetector(bool status)
    {
        upperBodyHitDetector.enabled = status;
    }

    private void EnableLowerBodyHitDetector(bool status)
    {
        lowerBodyHitDetector.enabled = status;
    }
    
    
    //For Platform detectors
    private void EnableUpperBodyPlatformDetector(bool status)
    {
        upperBodyPlatformDetector.enabled = status;
    }
    
    private IEnumerator DisableDodgerollImmuneDamage()
    {
        yield return new WaitForSeconds(immunityTime);

        EnableUpperBodyHitDetector(true);
        EnableLowerBodyHitDetector(true);
    }
    private IEnumerator SetCanWallJumpToFalse()
    {
        yield return new WaitForSeconds(wallJumpDelay);
        
        canWallJump = false;
    }
    
    private IEnumerator SetDroppedFromStairToFalse()
    {
        yield return new WaitForSeconds(.5f);
        droppedFromStair = false;
    }

    private IEnumerator ResetWallJump()
    {
        yield return new WaitForSeconds(leftOrRightJumpCooldownTime);

        if (jumpedToLeft)
            jumpedToLeft = false;
        
        else if (jumpedToRight)
            jumpedToRight = false;
    }

    private IEnumerator ResetGravityScale()
    {
        yield return new WaitForSeconds(0.5f);
        
        rb.gravityScale = gravity;
    }
    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.collider.CompareTag("Solid Ground"))
    //     {
    //         //animCon.OnLanding();
    //     }
    // }
    private RaycastHit2D TouchingPlatformCheck()
    {
        //Offset incase of uneven terrain
        float extraHeightTest = .1f;
        float extraWidthTest = .1f;
     

        //Draw BoxCast ground check
        RaycastHit2D rayCastHit = Physics2D.BoxCast(lowerBodyPlatformDetector.bounds.center, lowerBodyPlatformDetector.bounds.size + new Vector3(extraWidthTest, extraHeightTest, 0f), 0f, Vector2.down, 0f, platformLayerMask);
        //DrawRayCast(rayCastHit, extraHeightTest, extraWidthTest);
       
        return rayCastHit;
    }

    private void DrawRayCast(RaycastHit2D rayCastHit, float extraHeightTest, float extraWidthTest)
    {
         Color rayColor;
         if (rayCastHit.collider != null)
         {
             rayColor = Color.green;
         }
         else
         {
             rayColor = Color.red;
         }
        
        //See boxcast gizmos
         Debug.DrawRay(lowerBodyPlatformDetector.bounds.center + new Vector3(lowerBodyPlatformDetector.bounds.extents.x, 0f), Vector2.down * (lowerBodyPlatformDetector.bounds.extents.y + extraHeightTest), rayColor);
         Debug.DrawRay(lowerBodyPlatformDetector.bounds.center - new Vector3(lowerBodyPlatformDetector.bounds.extents.x, 0f), Vector2.down * (lowerBodyPlatformDetector.bounds.extents.y + extraHeightTest), rayColor);
         Debug.DrawRay(lowerBodyPlatformDetector.bounds.center - new Vector3(lowerBodyPlatformDetector.bounds.extents.x, lowerBodyPlatformDetector.bounds.extents.y), Vector2.right, rayColor);
    }
    
    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawCube(new Vector3(lowerBodyPlatformDetector.bounds.center.x, lowerBodyPlatformDetector.bounds.center.y + (2 * lowerBodyPlatformDetector.bounds.extents.y) + .005f, 0f), new Vector3(.5f, lowerBodyPlatformDetector.bounds.size.y, 0f));
    // }
}