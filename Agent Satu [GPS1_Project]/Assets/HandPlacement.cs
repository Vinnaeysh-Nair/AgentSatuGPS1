using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.U2D.IK;
using Vector2 = UnityEngine.Vector2;

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
        //PlaceHands();
    }

    private void PlaceHands()
    {
        leftHandIKTarget.position = leftHandPlacement.position;
        rightHandIKTarget.position = rightHandPlacement.position;
    }
}
