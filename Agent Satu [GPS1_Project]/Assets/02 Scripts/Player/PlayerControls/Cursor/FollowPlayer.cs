using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    //Components
    [SerializeField] private PlayerController controller;
    private Transform followTarget;
    
    
    //Fields
    //[SerializeField] [Range(0f, 1f)] private float offsetY;   (offset crouch height if needed)
    private bool wasCrouching;
    [SerializeField] private Vector2 followOffset;
    [SerializeField] private float crouchHeight;

    void Start()
    {
        followTarget = transform.parent;
    }
    void FixedUpdate()
    {
        Follow();
        if (controller.GetPlayerIsCrouching() && controller.GetGrounded() || controller.PlayerIsDodgerolling)
        {
            AdjustPos();
        }
    }

    //Normal pos when player not crouching
    private void Follow()
    {
        wasCrouching = false;
        transform.position = (Vector2) followTarget.position + followOffset;
    }
    
    
    //When player crouches, lower position
    private void AdjustPos()
    {
        Vector2 posBeforeCrouch = transform.position;
        
        if (!wasCrouching)
        {
            wasCrouching = true;
            transform.position = new Vector2(posBeforeCrouch.x, posBeforeCrouch.y + crouchHeight);
        }
    }
}
