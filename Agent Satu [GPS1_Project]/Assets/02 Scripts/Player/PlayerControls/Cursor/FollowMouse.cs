using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    //Components
    [SerializeField] private CrosshairAiming aim;

    
    void Update()
    {
       transform.position = aim.GetMousePos();
    }
}
