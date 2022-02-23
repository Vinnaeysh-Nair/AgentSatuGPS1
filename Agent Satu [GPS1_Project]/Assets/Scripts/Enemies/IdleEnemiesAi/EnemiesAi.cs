using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesAi : MonoBehaviour
{
    public GameObject visionCone;
    private bool playerInsideArea = false;
    //public bool checkIfBurstFire = false;
    public EnemyWeapon_Guns enemyGun;
    //private OverallHp overallHp;
    //private int enemiesHp;

    //public BoxCollider2D boxCollider;

    void Awake()
    {
       //overallHp = GetComponent<OverallHp>();
    }
    
    void Update()
    {
        //enemiesHp = overallHp.GetOverallHp();
        //Debug.Log(playerPosition);
        if (playerInsideArea)
        {
            enemyGun.StartShooting();
        }

        /*if (enemiesHp <= 0)
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
        Destroy(gameObject);
    }*/
}
