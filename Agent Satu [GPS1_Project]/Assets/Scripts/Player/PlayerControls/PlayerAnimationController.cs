using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator anim;

    public delegate void OnCrouchEnd();
    public static OnCrouchEnd onCrouchEndDelegate;

    public void OnMoving(float speed)
    {
        anim.SetFloat("Speed", speed);
    }

    public void OnCrouching()
    {
        anim.SetBool("IsCrouching", true);
    }
    
    public void OnCrouchReleasing()
    {
        anim.SetBool("IsCrouching", false);
    }
    
    public void OnDodgerolling()
    {
        anim.SetBool("IsDodgerolling", true);
    }
    
    public void OnDodgerollEnd()
    {
        anim.SetBool("IsDodgerolling", false);
        
        //Reset dodgeroll status in PlayerController
        if (onCrouchEndDelegate != null)
        {
            onCrouchEndDelegate.Invoke();
        }
    }

    public void OnJumping()
    {
        anim.SetTrigger("IsJumping");
    }
    // public void OnJumping()
    // {
    //     anim.SetBool("IsJumping", true);
    // }
    //
    // public void OnJumpEnd()
    // {
    //     anim.SetBool("IsJumping", false);
    // }
}
