using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesAi : MonoBehaviour
{
    public GameObject visionCone;
    private bool playerInsideArea = false;
    public bool checkIfBurstFire = false;
    public EnemyWeapon_Guns enemyGun;

    void Start()
    {
        //Debug.Log(playerInsideArea);
        
    }

    void Update()
    {
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
}
