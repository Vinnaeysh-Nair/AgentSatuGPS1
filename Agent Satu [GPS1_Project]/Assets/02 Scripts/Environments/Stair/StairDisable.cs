using UnityEngine;

public class StairDisable : Stair
{
    private EdgeCollider2D _stairCollider;

    private bool canDisable = false;
    public bool disabledAtStart = false;

    void Awake()
    {
        //Different way of getting stairCollider from Stair base class
        _stairCollider = GetComponent<EdgeCollider2D>();
        
        Transform playerBody = GameObject.FindGameObjectWithTag("PlayerBody").GetComponent<Transform>();
        platformDetectors = playerBody.Find("Detectors/PlatformDetectors").GetComponentsInChildren<BoxCollider2D>();

        if (disabledAtStart)
        {
            foreach (BoxCollider2D detectors in platformDetectors)
            {
                Physics2D.IgnoreCollision(detectors, _stairCollider);
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
        {
            canDisable = true;
        }
            
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canDisable = false;
        }
    }
    

    private void DisableStair()
    {
        foreach (BoxCollider2D detectors in platformDetectors)
        {
            Physics2D.IgnoreCollision(detectors, _stairCollider);
        }
    }
}
