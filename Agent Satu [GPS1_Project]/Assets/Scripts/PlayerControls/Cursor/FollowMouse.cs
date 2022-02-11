using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    //Components
    public GameObject crosshairSprite;
    public CrosshairAiming aim;

    void FixedUpdate()
    {
        crosshairSprite.transform.position = aim.GetMousePos();
    }
}
