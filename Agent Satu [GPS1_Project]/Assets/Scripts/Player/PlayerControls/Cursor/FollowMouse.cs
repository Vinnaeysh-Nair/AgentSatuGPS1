using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    //Components
    [SerializeField] private CrosshairAiming aim;

    
    void FixedUpdate()
    {
       transform.position = aim.GetMousePos();
    }
}
