using Unity.VisualScripting;
using UnityEngine;

//Attached to CameraTargetPos object
public class CameraTarget : MonoBehaviour
{
    //Components
    [SerializeField] private CrosshairAiming aim;
    private Transform playerBody;

    //Field
    [SerializeField] private float thresholdX;
    [SerializeField] private float thresholdYUp;
    [SerializeField] private float thresholdYDown;
    

    void Awake()
    {
        playerBody = transform.Find("/Player/PlayerBody").GetComponent<Transform>();
        aim = transform.Find("/Player/PlayerBody/WeaponPivot").GetComponent<CrosshairAiming>();
    }
    void FixedUpdate()
    {
        Vector2 playerPos = playerBody.position;
        
        //Find position between player and cursor
        Vector2 targetPos =  (playerPos + aim.GetMousePos())/2;
        
        
        //Limit by threshold amount
        targetPos.x = Mathf.Clamp(targetPos.x, -thresholdX + playerPos.x, thresholdX + playerPos.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -thresholdYDown + playerPos.y, thresholdYUp + playerPos.y);
        
        
        transform.position = targetPos;
    }
}
