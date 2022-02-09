using UnityEngine;

//Attached to CameraTargetPos object
public class CameraTarget : MonoBehaviour
{
    //Components
    public CrosshairAiming aim;
    public Transform player;
    
    //Field
    [SerializeField] private float threshold;
    
    void FixedUpdate()
    {
        Vector2 targetPos = (Vector2) player.position + aim.GetMousePos();

        targetPos.x = Mathf.Clamp(targetPos.x, -threshold + player.position.x, threshold + player.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -threshold + player.position.y, threshold + player.position.y);

        transform.position = targetPos;
    }
}
