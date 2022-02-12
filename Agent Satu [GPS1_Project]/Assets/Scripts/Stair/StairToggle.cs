using UnityEngine;

public class StairToggle : MonoBehaviour
{
    private EdgeCollider2D stairCollider;

    void Awake()
    {
        stairCollider = GetComponentInParent<EdgeCollider2D>();

        stairCollider.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (gameObject.CompareTag("Stair Start")) 
            stairCollider.enabled = true;
        else if (gameObject.CompareTag("Stair End"))
            stairCollider.enabled = false;
    }
}
