using UnityEngine;

public class CrosshairAiming : MonoBehaviour
{
    //Components;
    private Camera cam;
    public Transform pivotTransform;
    
    //Fields
    private Vector2 mousePos;

    public Vector2 GetMousePos()
    {
        return mousePos;
    }
    
    void Awake()
    {
        cam = Camera.main;
    }
    
    //Fix camera not following after a certain distance from spawn position
    void Update()
    {
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
            pivotTransform.eulerAngles = new Vector3(180f, 0f, -angleTowards);
        }
        else 
        {
            //Normal rotation
            pivotTransform.eulerAngles = new Vector3(0f, 0f, angleTowards);
        }
    }
    
}
