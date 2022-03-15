using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator anim;

    public void OnRunning(float speed)
    {
        anim.SetFloat("Speed", speed);
    }
}
