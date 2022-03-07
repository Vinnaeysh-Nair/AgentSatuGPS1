using UnityEngine;

public class AimAtTarget : MonoBehaviour
{
    [SerializeField] private Transform targetPoint;

    void FixedUpdate()
    {
        PointToTarget();
    }
    
    private void PointToTarget()
    {
        Vector2 lookDir = targetPoint.position - transform.position;
        float angleTowards = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        
        //angle correction
        angleTowards += 90f;

        transform.eulerAngles = new Vector3(0f, 0f, angleTowards);
    }
}
