using System.Net.Sockets;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    //Components
    //public Transform followPoint;
    [SerializeField] private PlayerController controller;

    //Fields
    //[SerializeField] [Range(0f, 1f)] private float offsetY;   (offset crouch height if needed)
    private bool wasCrouching;
    private Transform followTarget;
    [SerializeField] private Vector2 followOffset;
    [SerializeField] private float crouchHeight;

    void Start()
    {
        followTarget = transform.parent;
    }
    void Update()
    {
        Follow();
        if (controller.GetPlayerIsCrouching() && controller.GetGrounded())
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
