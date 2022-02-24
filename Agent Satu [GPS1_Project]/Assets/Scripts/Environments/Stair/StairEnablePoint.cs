using UnityEngine;

public class StairEnablePoint : MonoBehaviour
{
    private EdgeCollider2D stairCollider;
    private BoxCollider2D[] platformDetectors;

    
    void Awake()
    {
        stairCollider = GetComponentInParent<EdgeCollider2D>();
        platformDetectors = transform.Find("/Player/PlayerBody/Detectors/PlatformDetectors").GetComponentsInChildren<BoxCollider2D>();
    }
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            foreach (BoxCollider2D detectors in platformDetectors)
            {
                Physics2D.IgnoreCollision(detectors, stairCollider, false);
            }
        }
    }
}
