using UnityEngine;

public class StairEnablePoint : Stair
{
    
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
