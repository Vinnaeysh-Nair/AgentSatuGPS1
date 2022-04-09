using UnityEngine;

//Attached to CameraTargetPos object
public class CameraTarget : MonoBehaviour
{
    //Components
    [Header("Scene Boundaries")]
    [SerializeField] private Transform leftBarrier;
    [SerializeField] private Transform rightBarrier;
    [SerializeField] private Transform downBarrier;
    
    //Ref
    private Transform playerBody;
    private CrosshairAiming aim;
    
    

    //Fields
    [Header("Settings")]
    [SerializeField] private float thresholdX;
    [SerializeField] private float thresholdYUp;
    [SerializeField] private float thresholdYDown;
    [SerializeField] private float downBarrierCameraOffsetY;
    [SerializeField] [Range(0f, 1f)] private float cameraPosLerp = .5f;

    private float leftBarrierX;
    private float rightBarrierX;
    
    private float downBarrierY;

    
    private Transform followPlayer;
    
    void Start()
    { 
        playerBody = GameObject.FindGameObjectWithTag("PlayerBody").GetComponent<Transform>();
        aim = playerBody.Find("WeaponPivot").GetComponent<CrosshairAiming>();

        followPlayer = transform.parent;

        //Get barriers' sizes
        leftBarrierX = leftBarrier.position.x + leftBarrier.GetComponent<BoxCollider2D>().bounds.extents.x/2;
        rightBarrierX = rightBarrier.position.x - rightBarrier.GetComponent<BoxCollider2D>().bounds.extents.x/2;
        downBarrierY = downBarrier.position.y - downBarrier.GetComponent<BoxCollider2D>().bounds.extents.y / 2 + downBarrierCameraOffsetY;
    }
    
    void FixedUpdate()
    {
        Vector2 playerPos = playerBody.position;
        
        //Follow player, allow thresholdYDown to be relative to playerPos
        followPlayer.position = playerPos;
        
        //Find middle point of player and cursor
        Vector2 targetPos =  (playerPos + aim.GetMousePos())/2;
        
        //Limit by threshold amount
        targetPos.x = Mathf.Clamp(targetPos.x, -thresholdX + playerPos.x, thresholdX + playerPos.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -thresholdYDown + playerPos.y, thresholdYUp + playerPos.y);

        //transform.position = targetPos;

        Vector2 lerpedPos;
        lerpedPos.x = Mathf.Lerp(transform.position.x, targetPos.x, cameraPosLerp);
        lerpedPos.y = Mathf.Lerp(transform.position.y, targetPos.y, cameraPosLerp);
        transform.position = lerpedPos;
        
        KeepCameraInBounds();
    }


    private void KeepCameraInBounds()
    {
        Vector2 currPos = transform.position;
        Vector2 newPos = new Vector2(currPos.x, currPos.y);
        
        if (currPos.x <= leftBarrierX)
        {
            newPos.x = leftBarrierX + .1f;
        }
        else if (currPos.x >= rightBarrierX)
        {
            newPos.x = rightBarrierX - .1f;
        }
        
        if (currPos.y <= downBarrierY)
        {
           newPos.y = downBarrierY + .1f;
        }

        transform.position = newPos;
    }
}
