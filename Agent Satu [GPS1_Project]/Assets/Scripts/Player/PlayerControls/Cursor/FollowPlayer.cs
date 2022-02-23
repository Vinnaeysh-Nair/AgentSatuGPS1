using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    //Components
    public GameObject followPoint;
    public PlayerController controller;

    //Fields
    //[SerializeField] [Range(0f, 1f)] private float offsetY;   (offset crouch height if needed)
    private bool wasCrouching;
    [SerializeField] private float crouchHeight;
    
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
        transform.position = followPoint.transform.position;
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
