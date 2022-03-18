using UnityEngine;

public class Stair : MonoBehaviour
{ 
    protected EdgeCollider2D stairCollider;
    protected BoxCollider2D[] platformDetectors;
    void Awake()
    {
        stairCollider = GetComponentInParent<EdgeCollider2D>();
        
        Transform playerBody = GameObject.FindGameObjectWithTag("PlayerBody").GetComponent<Transform>();
        platformDetectors = playerBody.Find("Detectors/PlatformDetectors").GetComponentsInChildren<BoxCollider2D>();
    }
}
