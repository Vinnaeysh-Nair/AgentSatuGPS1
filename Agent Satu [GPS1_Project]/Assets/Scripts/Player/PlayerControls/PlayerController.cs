using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //[Range (0, 1)] [SerializeField] private float crouchSlowMultiplier = 0.5f;
    
    //Components    
    //public PlayerAnimationController animCon;
    public CrosshairAiming aim;
    private Rigidbody2D rb;
    private BoxCollider2D platformCollider;
    private BoxCollider2D upperBodyCollider;
    private BoxCollider2D lowerBodyCollider;

    
    //Fields
    //General movement fields
    [Header("General Movement")]
    [SerializeField] private float horizontalMoveSpeed = 20f;
    [SerializeField] private float jumpForce = 30f;
    [SerializeField] private float gravity = 9f;
    [SerializeField] [Range(0f, 1f)] private float horizontalLerp = .3f;
    [SerializeField] [Range(0f, 1f)] private float verticalLerp = .8f;
    private bool grounded = true;
    private bool facingRight = true;
    private bool wasCrouching = false;

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
    private float wallJumpDelay = .08f;
    private bool jumpedToLeft = false;
    private bool jumpedToRight = false;
    

    
    //Groundcheck fields
    [SerializeField] private LayerMask platformLayerMask;
    
    //Getter
    public bool GetGrounded()
    {
        return grounded;
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        //Set to customized gravity
        rb.gravityScale = gravity;
        
        //Find Ground collider
        platformCollider = transform.Find("Detectors/PlatformDetector").GetComponent<BoxCollider2D>();

        //Find Hit colliders, these colliders should be triggers
        BoxCollider2D[] hitColliders = transform.Find("Detectors/HitDetector").GetComponentsInChildren<BoxCollider2D>();
        upperBodyCollider = hitColliders[0];
        lowerBodyCollider = hitColliders[1];
    }

    //To detect if touching anything on platformLayerMask
    void Update()
    {
        RaycastHit2D collidedPlatform = TouchingPlatformCheck();
        
        //Nothing detected
        if (collidedPlatform.collider == null) return;
        
        //When touching ground
        if (collidedPlatform.normal == Vector2.left || collidedPlatform.normal == Vector2.right)
        {
            canWallJump = true;
            return;
        }
        
        //When touching anything other than ground
        grounded = true;
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
            //Reduce gravity, reset after a timer
            rb.gravityScale *= 1 - wallJumpGravityReduction;
            StartCoroutine(ResetGravityScale());
            
            //Create overlapBox to detect collision on left and right of the player
            Collider2D leftBox = Physics2D.OverlapBox(new Vector2(platformCollider.bounds.center.x - platformCollider.bounds.extents.x - .15f, platformCollider.bounds.center.y), new Vector2(.1f, .1f), 0f);
            Collider2D rightBox = Physics2D.OverlapBox(new Vector2(platformCollider.bounds.center.x + platformCollider.bounds.extents.x + .17f, platformCollider.bounds.center.y), new Vector2(.1f, .1f), 0f);
            
            //Wall jump direction according to side detected
            int wallJumpDir = 0;
            int vMoveControl = 1;
            
            if (rightBox != null && !jumpedToLeft)
            {
                wallJumpDir = -1;
                jumpedToLeft = true;
                StartCoroutine(ResetWallJump());
            }
            else if (leftBox != null && !jumpedToRight)
            {
                wallJumpDir = 1;
                jumpedToRight = true;
                StartCoroutine(ResetWallJump());
            }
            else
            {
                //If isn't able to wall jump, set x and y velocity to 0
                wallJumpDir = 0;
                vMoveControl = 0;
            }
            horizontalMove = xWallJumpForce * wallJumpDir;
            verticalMove = yWallJumpForce * vMoveControl;

            StartCoroutine(SetCanWallJumpToFalse());
        }
        //Crouch
        else if (crouch && grounded)
        {
            //horizontalMove = horizontalMoveDir * horizontalMoveSpeed * crouchSlowMultiplier;
            wasCrouching = true;
            horizontalMove = 0f;
            
            //Disable upper body collision
            EnableUpperBodyCollider(false);
            
            //animCon.OnCrouching();
        }
        //Dodgeroll
        else if (dodgeroll && grounded)
        {
            //Disable collisions
            EnableUpperBodyCollider(false);
            EnableLowerBodyCollider(false);
    
            //Apply cooldown to dodgeroll
            if (Time.time > nextDodgerollTime)
            {
                StartCoroutine(DisableDodgerollImmuneDamage());
                
                //Direction is according to player's horizontal movement
                if (horizontalMove < 0f || !facingRight)
                {
                    horizontalMove = -dodgerollHorizontal;
                }
                else if (horizontalMove >= 0f || facingRight)
                {
                    horizontalMove = dodgerollHorizontal;
                }
                verticalMove = dodgerollVertical;
                            
                //Add animation
                        
                nextDodgerollTime = Time.time + cooldownTime;
            }
        }
        //Default state
        else
        {
            horizontalMove = horizontalMoveDir * horizontalMoveSpeed;

            
            //After crouching, re-enable upper body collider
            if (wasCrouching)
            {
                EnableUpperBodyCollider(true);
                wasCrouching = false;
            }
            
            //animCon.OnCrouchReleasing();
            //animCon.OnRunning();
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
    
    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180f, 0);
    }

    private void EnableUpperBodyCollider(bool status)
    {
        upperBodyCollider.enabled = status;
    }

    private void EnableLowerBodyCollider(bool status)
    {
        lowerBodyCollider.enabled = status;
    }
    
    private IEnumerator DisableDodgerollImmuneDamage()
    {
        yield return new WaitForSeconds(immunityTime);

        EnableUpperBodyCollider(true);
        EnableLowerBodyCollider(true);
    }
    private IEnumerator SetCanWallJumpToFalse()
    {
        yield return new WaitForSeconds(wallJumpDelay);
        
        canWallJump = false;
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
        RaycastHit2D rayCastHit = Physics2D.BoxCast(platformCollider.bounds.center, platformCollider.bounds.size + new Vector3(extraWidthTest, extraHeightTest, 0f), 0f, Vector2.down, 0f, platformLayerMask);
        // Color rayColor;
        // if (rayCastHit.collider != null)
        // {
        //     rayColor = Color.green;
        // }
        // else
        // {
        //     rayColor = Color.red;
        // }
        
        //See boxcast gizmos
        // Debug.DrawRay(platformCollider.bounds.center + new Vector3(platformCollider.bounds.extents.x, 0f), Vector2.down * (platformCollider.bounds.extents.y + extraHeightTest), rayColor);
        // Debug.DrawRay(platformCollider.bounds.center - new Vector3(platformCollider.bounds.extents.x, 0f), Vector2.down * (platformCollider.bounds.extents.y + extraHeightTest), rayColor);
        // Debug.DrawRay(platformCollider.bounds.center - new Vector3(platformCollider.bounds.extents.x, platformCollider.bounds.extents.y), Vector2.right, rayColor);

        return rayCastHit;
    }
    
    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawCube(new Vector2(platformCollider.bounds.center.x + platformCollider.bounds.extents.x + .15f, platformCollider.bounds.center.y), new Vector3(.1f, .1f, 0f));
    //     Gizmos.DrawCube(new Vector2(platformCollider.bounds.center.x - platformCollider.bounds.extents.x - .15f, platformCollider.bounds.center.y), new Vector3(.1f, .1f, 0f));
    // }
}