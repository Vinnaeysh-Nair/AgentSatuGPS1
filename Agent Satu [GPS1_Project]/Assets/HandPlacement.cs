
using UnityEngine;


public class HandPlacement : MonoBehaviour
{
    [SerializeField] private Transform leftHandIKTarget;
    [SerializeField] private Transform rightHandIKTarget;
    
    private Transform leftHandPlacement;
    private Transform rightHandPlacement;

    private bool startPlacing = false;



    void OnEnable()
    {
        startPlacing = true;

        if (leftHandPlacement == null && rightHandPlacement == null)
        {
            Transform handPlacements = transform.Find("HandPlacements");
            leftHandPlacement = handPlacements.GetChild(0).GetComponent<Transform>();
            rightHandPlacement = handPlacements.GetChild(1).GetComponent<Transform>();
        }

        PlaceHands();
    }

    private void OnDisable()
    {
        startPlacing = false;
    }

    private void FixedUpdate()
    {
        if (!startPlacing) return;
        PlaceHands();
    }

    private void PlaceHands()
    {
        if (Vector2.Distance(leftHandIKTarget.position, leftHandPlacement.position) > 0f)
        {
            
            startPlacing = false;
        }
        

        leftHandIKTarget.position = leftHandPlacement.position;
        rightHandIKTarget.position = rightHandPlacement.position;
    }
}
