using UnityEngine;

public class StairDisable : MonoBehaviour
{
    private EdgeCollider2D stairCollider;
    private BoxCollider2D platformCollider;

    private bool canDisable = false;
    public bool disabledAtStart = false;

    void Awake()
    {
        stairCollider = GetComponentInParent<EdgeCollider2D>();
        platformCollider = transform.Find("/Player/PlayerBody/PlatformDetector").GetComponent<BoxCollider2D>();

        //No collision at the start
        if(disabledAtStart)
            Physics2D.IgnoreCollision(platformCollider, stairCollider);
    }
    
    void Update()
    {
        if (!canDisable) return;
        
        if (Input.GetButtonDown("Crouch"))
        {
            Physics2D.IgnoreCollision(platformCollider, stairCollider);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            canDisable = true;
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
            canDisable = false;
    }
}
