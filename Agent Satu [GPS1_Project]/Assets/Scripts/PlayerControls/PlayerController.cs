using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //[Range (0, 1)] [SerializeField] private float crouchSlowMultiplier = 0.5f;
    
    //Components    
    public Rigidbody2D rb;
    //public PlayerAnimationController animCon;
    public CrosshairAiming aim;

    
    //Fields
    //General movement fields
    [SerializeField] private float horizontalMoveSpeed = 20f;
    [SerializeField] private float jumpForce = 30f;
    private bool grounded = true;
    private bool facingRight = true;
    
    //Dodgeroll fields
    [SerializeField] private float dodgerollHorizontal = 50f;
    [SerializeField] private float dodgerollVertical = 10f;
    [SerializeField] private float cooldownTime = 2f;
    private float nextDodgerollTime = 0f;
    
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
        //Crouch
        else if (crouch && grounded)
        {
            //horizontalMove = horizontalMoveDir * horizontalMoveSpeed * crouchSlowMultiplier;
           
            horizontalMove = 0f;
            //animCon.OnCrouching();
        }
        //Dodgeroll
        else if (dodgeroll && grounded)
        {
            //Apply cooldown to dodgeroll
            if (Time.time > nextDodgerollTime)
            {
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
        else
        {
            horizontalMove = horizontalMoveDir * horizontalMoveSpeed;

            //animCon.OnCrouchReleasing();
            //animCon.OnRunning();
        }
        
        //Apply velocity
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            grounded = true;
            //animCon.OnLanding();
        }
    }
}
