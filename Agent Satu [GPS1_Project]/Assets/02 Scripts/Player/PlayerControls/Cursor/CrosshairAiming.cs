using UnityEngine;

public class CrosshairAiming : MonoBehaviour
{
    //Components;
    private Camera cam;
    private Transform pivotTransform;
    
    //UI
    private PauseMenu pauseMenu;
    
    //Fields
    private Vector2 mousePos;
    public float followAngleOffset;

    public Vector2 GetMousePos()
    {
        return mousePos;
    }
    
    void Awake()
    {
        cam = Camera.main;
    }

    void Start()
    {
        pivotTransform = GetComponent<Transform>();
        pauseMenu = PauseMenu.Instance;
    }
    
    //Fix camera not following after a certain distance from spawn position
    void FixedUpdate()
    {
        if (pauseMenu.gameIsPaused) return;
        
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        PointToMouse();
    }
    
    private void PointToMouse()
    {
        Vector2 lookDir = mousePos - (Vector2) pivotTransform.position;
        float angleTowards = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        
        //If pointing to left side, invert the pivot's x rotation and angleTowards
        //to accomodate sprite rotations
        if (angleTowards > 90f || angleTowards < -90f)
        {
            //Inverted rotation
            pivotTransform.eulerAngles = new Vector3(180f, 0f, -angleTowards - followAngleOffset);
        }
        else 
        {
            //Normal rotation
            pivotTransform.eulerAngles = new Vector3(0f, 0f, angleTowards - followAngleOffset);
        }
    }
    
}
