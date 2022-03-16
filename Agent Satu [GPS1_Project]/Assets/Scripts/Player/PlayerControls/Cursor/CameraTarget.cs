using UnityEngine;

//Attached to CameraTargetPos object
public class CameraTarget : MonoBehaviour
{
    //Components
    [Header("Scene Boundaries")]
    [SerializeField] private Transform leftBarrier;
    [SerializeField] private Transform rightBarrier;
    
    private CrosshairAiming aim;
    private Transform playerBody;
    

    //Fields
    [Header("Settings")]
    [SerializeField] private float thresholdX;
    [SerializeField] private float thresholdYUp;
    [SerializeField] private float thresholdYDown;
    
    private float leftBarrierX;
    private float rightBarrierX;

    
    
    void Start()
    {
        playerBody = transform.Find("/Player/PlayerBody").GetComponent<Transform>();
        aim = transform.Find("/Player/PlayerBody/WeaponPivot").GetComponent<CrosshairAiming>();

        //Get barriers' sizes
        leftBarrierX = leftBarrier.position.x + leftBarrier.GetComponent<BoxCollider2D>().bounds.extents.x/2;
        rightBarrierX = rightBarrier.position.x - rightBarrier.GetComponent<BoxCollider2D>().bounds.extents.x/2;
    }
    
    void FixedUpdate()
    {
        Vector2 playerPos = playerBody.position;
        
        //Find middle point of player and cursor
        Vector2 targetPos =  (playerPos + aim.GetMousePos())/2;
        
        
        //Limit by threshold amount
        targetPos.x = Mathf.Clamp(targetPos.x, -thresholdX + playerPos.x, thresholdX + playerPos.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -thresholdYDown + playerPos.y, thresholdYUp + playerPos.y);

        transform.position = targetPos;
        KeepCameraInBounds();
    }

    private void KeepCameraInBounds()
    {
        Vector2 currPos = transform.position;
        if (currPos.x <= leftBarrierX)
        {
            transform.position = new Vector2(leftBarrierX + .1f, currPos.y);
        }
        else if (currPos.x >= rightBarrierX)
        {
            transform.position = new Vector2(rightBarrierX - .1f, currPos.y);
        }
    }
}
