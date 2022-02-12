using UnityEngine;

//Attached to CameraTargetPos object
public class CameraTarget : MonoBehaviour
{
    //Components
    public CrosshairAiming aim;
    public Transform playerBody;
    
    //Field
    [SerializeField] private float threshold;
    
    void FixedUpdate()
    {
        Vector2 targetPos = (Vector2) playerBody.position + aim.GetMousePos();

        targetPos.x = Mathf.Clamp(targetPos.x, -threshold + playerBody.position.x, threshold + playerBody.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -threshold + playerBody.position.y, threshold + playerBody.position.y);

        transform.position = targetPos;
    }
}
