using System;
using Unity.VisualScripting;
using UnityEngine;

public class StairDisable : MonoBehaviour
{
    private EdgeCollider2D stairCollider;
    private BoxCollider2D[] platformDetectors;

    private bool canDisable = false;
    public bool disabledAtStart = false;

    void Awake()
    {
        stairCollider = GetComponentInParent<EdgeCollider2D>();
        platformDetectors = transform.Find("/Player/PlayerBody/Detectors/PlatformDetectors").GetComponentsInChildren<BoxCollider2D>();

        //No collision at the start
        if (disabledAtStart)
        {
            foreach (BoxCollider2D detectors in platformDetectors)
            {
                Physics2D.IgnoreCollision(detectors, stairCollider);
            }
        }
    }
    
    void Update()
    {
        if (!canDisable) return;
        
        if (Input.GetButtonDown("Crouch"))
        {
            DisableStair();
        }
    }

    
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            canDisable = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            canDisable = false;
    }
    

    private void DisableStair()
    {
        foreach (BoxCollider2D detectors in platformDetectors)
        {
            Physics2D.IgnoreCollision(detectors, stairCollider);
        }
    }
}
