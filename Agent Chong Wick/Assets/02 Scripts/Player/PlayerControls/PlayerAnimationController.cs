using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private SoundManager sManager;

    public delegate void OnCrouchEnd();
    public static OnCrouchEnd onCrouchEndDelegate;

    private void Start()
    {
        sManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    public void OnMoving()
    {
        anim.SetFloat("Speed", Mathf.Abs(PlayerMovement.horizontalMove));
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
        sManager.PlayEffect("DashSound");
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
        anim.SetBool("IsJumping", true);
    }
    
    public void OnJumpEnd()
    {
        anim.SetBool("IsJumping", false);
    }
}
