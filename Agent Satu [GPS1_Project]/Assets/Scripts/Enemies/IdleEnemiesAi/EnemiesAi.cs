using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesAi : MonoBehaviour
{
    public GameObject visionCone;
    private bool playerInsideArea = false;
    public bool checkIfBurstFire = false;
    public EnemyWeapon_Guns enemyGun;
    //public OverallHp overallHp;
    //private int tempHp;
    //public BoxCollider2D boxCollider;

    void Awake()
    {
        //tempHp = overallHp.GetOverallHp();
    }
    
    void Update()
    {
        //tempHp = overallHp.GetOverallHp();
        //Debug.Log(playerPosition);
        if (playerInsideArea)
        {
            if (checkIfBurstFire)
            {
                //burst fire mode doesnt work for now
                //Debug.Log("burst fire");
                enemyGun.StartBurstShooting();
            }
            else
            {
                //Debug.Log("single fire");
                enemyGun.StartShooting();
            }
        }

        /*if(tempHp <= 0)
        {
            disableCollider();
        }*/
    }

    void OnTriggerEnter2D(Collider2D visionCone)
    {
        if (visionCone.gameObject.tag == "Player")
        {
            playerInsideArea = true;
            //Debug.Log("Player Enters vision area");
            //Debug.Log(playerInsideArea);
        }
    }

    void OnTriggerExit2D(Collider2D visionCone)
    {
        if (visionCone.gameObject.tag == "Player")
        {
            playerInsideArea = false;
            //Debug.Log("Player Enters vision area");
            //Debug.Log(playerInsideArea);
        }
    }

    /*void disableCollider()
    {
        boxCollider.enabled = false;
    }*/
}
