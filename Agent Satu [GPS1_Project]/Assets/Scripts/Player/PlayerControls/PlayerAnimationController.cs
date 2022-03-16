using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator anim;

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
    }
}
