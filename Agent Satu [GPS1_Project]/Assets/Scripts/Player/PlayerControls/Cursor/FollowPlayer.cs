using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    //Components
    public GameObject playerBody;
    public PlayerMovement playerMovement;
    public PlayerController controller;

    //Fields
    //[SerializeField] [Range(0f, 1f)] private float offsetY;   (offset crouch height if needed)
    private bool wasCrouching;
    private float crouchHeight;

    void Awake()
    {
        crouchHeight = playerMovement.GetPlayerHeight() * 0.5f;
    }
    
    void Update()
    {
        Follow();
        if (playerMovement.GetCrouch() && controller.GetGrounded())
        {
            AdjustPos();
        }
    }

    //Normal pos when player not crouching
    private void Follow()
    {
        wasCrouching = false;
        transform.position = playerBody.transform.position;
    }
    
    
    //When player crouches, lower position
    private void AdjustPos()
    {
        Vector2 prevPivotPos = transform.position;
        
        if (!wasCrouching)
        {
            wasCrouching = true;
            transform.position = new Vector2(prevPivotPos.x, prevPivotPos.y - crouchHeight);
        }
    }
}
