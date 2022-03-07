using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    //Components
    [SerializeField] private CrosshairAiming aim;
    [SerializeField] private Vector2 offSetPos;

    
    void FixedUpdate()
    {
       transform.position = aim.GetMousePos() + offSetPos;
    }
}
