using UnityEngine;

public class StairEnable : MonoBehaviour
{
    private EdgeCollider2D stairCollider;
    private BoxCollider2D platformCollider;

    void Awake()
    {
        stairCollider = GetComponentInParent<EdgeCollider2D>();
        platformCollider = transform.Find("/Player/PlayerBody/Detectors/PlatformDetector").GetComponent<BoxCollider2D>();
    }
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        Physics2D.IgnoreCollision(platformCollider, stairCollider, false);
    }
}
