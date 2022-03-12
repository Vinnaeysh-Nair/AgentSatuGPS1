using UnityEngine;

public class SynchGunMovements : MonoBehaviour
{
    private TargetPointMovement leftPoint;
    private TargetPointMovement rightPoint;

    void Start()
    {
        leftPoint = transform.GetChild(0).GetComponent<TargetPointMovement>();
        rightPoint = transform.GetChild(1).GetComponent<TargetPointMovement>();
    }

    public bool BothPointsInPosition()
    {
        if (leftPoint.GetInPosition() && rightPoint.GetInPosition())
        {
            return true;
        }
        return false;
    }
}
