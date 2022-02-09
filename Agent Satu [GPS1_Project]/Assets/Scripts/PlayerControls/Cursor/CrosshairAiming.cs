using UnityEngine;

public class CrosshairAiming : MonoBehaviour
{
    //Components;
    public Camera cam;
    public Transform pivotTransform;
    
    //Fields
    private Vector2 mousePos;

    public Vector2 GetMousePos()
    {
        return mousePos;
    }
    
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        PointToMouse();
    }
    
    private void PointToMouse()
    {
        Vector2 lookDir = mousePos - (Vector2) pivotTransform.position;
        
        float angleTowards = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        pivotTransform.eulerAngles = new Vector3(0f, 0f, angleTowards);
    }
}
