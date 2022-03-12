using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmToPlayerTracking : MonoBehaviour
{
    public Transform pivotTransform;
    public float followAngleOffset;
    public bool facingLeft = true;
    private PlayerMovement playerMovement;
    private Vector2 playerPosition;

    void Awake()
    {
        playerMovement = transform.Find("/Player/PlayerBody").GetComponent<PlayerMovement>();
    }

    //track player's Vector x and y
    void Update()
    {
        playerPosition = playerMovement.GetPlayerPos();
        PointToMouse();
    }

    private void PointToMouse()
    {
        Vector2 lookDir = (Vector2)pivotTransform.position - playerPosition;
        float angleTowards = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        //If pointing to left side, invert the pivot's x rotation and angleTowards
        //to accomodate sprite rotations
        if (facingLeft)
        {
            if (!(angleTowards > 90f || angleTowards < -90f))
            {
                //Normal rotation
                pivotTransform.eulerAngles = new Vector3(0f, 0f, angleTowards - followAngleOffset);
            }
        }
        else
        {
            if (angleTowards > 90f || angleTowards < -90f)
            {
                //Inverted rotation
                pivotTransform.eulerAngles = new Vector3(180f, 0f, -angleTowards - followAngleOffset);
            }
        }
    }
}
