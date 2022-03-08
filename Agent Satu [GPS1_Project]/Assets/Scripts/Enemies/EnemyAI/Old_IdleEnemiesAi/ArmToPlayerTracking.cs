using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmToPlayerTracking : MonoBehaviour
{
    public Transform pivotTransform;
    public float followAngleOffset;
    [SerializeField] bool isFacingRight = false;
    private Vector2 playerPosition;

    //reference to other scripts
    private PlayerMovement playerMovement;
    //private Enemy_Flipped enemyFlipped;

    void Awake()
    {
        playerMovement = transform.Find("/Player/PlayerBody").GetComponent<PlayerMovement>();
        //enemyFlipped = GetComponent<Enemy_Flipped>();
    }

    //track player's Vector x and y
    void Update()
    {
        //isFacingRight = enemyFlipped.detectFacingDirection();
        playerPosition = playerMovement.GetPlayerPos();
        PointToPlayer();
    }

    private void PointToPlayer()
    {
        Vector2 lookDir = (Vector2)pivotTransform.position - playerPosition;
        float angleTowards = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        //If pointing to left side, invert the pivot's x rotation and angleTowards
        //to accomodate sprite rotations
        if (isFacingRight)
        {
            if (angleTowards > 90f || angleTowards < -90f)
            {
                //Inverted rotation
                pivotTransform.eulerAngles = new Vector3(180f, 0f, -angleTowards - followAngleOffset);
            }
        }
        else
        {
            if (!(angleTowards > 90f || angleTowards < -90f))
            {
                //Normal rotation
                pivotTransform.eulerAngles = new Vector3(0f, 0f, angleTowards - followAngleOffset);
            }
        }
    }
}
