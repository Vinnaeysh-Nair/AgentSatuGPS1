using UnityEngine;

//Attached to CameraTargetPos object
public class CameraTarget : MonoBehaviour
{
    //Components
    [SerializeField] private CrosshairAiming aim;
    private Transform playerBody;

    //Field
    [SerializeField] private float threshold;
    

    void Awake()
    {
        playerBody = transform.Find("/Player/PlayerBody").GetComponent<Transform>();
        aim = transform.Find("/Player/PlayerBody/Pivots + Arms/LeftPivot/LeftArm").GetComponent<CrosshairAiming>();
    }
    void FixedUpdate()
    {
        Vector2 playerPos = playerBody.position;
        
        //Find position between player and cursor
        Vector2 targetPos =  (playerPos + aim.GetMousePos())/2;
        
        
        //Limit by threshold amount
        targetPos.x = Mathf.Clamp(targetPos.x, -threshold + playerPos.x, threshold + playerPos.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -threshold + playerPos.y, threshold + playerPos.y);
        
        
        transform.position = targetPos;
    }
}
