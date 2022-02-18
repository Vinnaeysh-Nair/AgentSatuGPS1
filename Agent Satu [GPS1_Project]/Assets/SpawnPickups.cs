using UnityEngine;

public class SpawnPickups : MonoBehaviour
{
    //Components
    public Transform[] picksUpSpawnedWhenDead;
    public float spawnForce = 1f;

    void Awake()
    {
        enabled = false;
    }
    void OnEnable()
    {
        SpawnItem();
    }
    
    private void SpawnItem()
    { 
        //Determine spawn health/pickups/none
        float outerRNG = Random.Range(0f, 1f);
        int spawnId = 0;
       
       
        
        
        if (outerRNG < 0.5f)
        {
            
            float innerRNG = Random.Range(0f, 1f);
           
            if (innerRNG < 0.2)
            {
                spawnId = 1;
            }
            else if (innerRNG < 0.4)
            {
                spawnId = 2;
            }
            else if (innerRNG < 0.6)
            {
                spawnId = 3;
            }    
            else if (innerRNG < 0.8)
            {
                spawnId = 4;
            }
            else
            {
                spawnId = 5;
            }
            //Spawn bulletspickups
            //Spawn with spawnId
           
        }
        else if (outerRNG < 0.8){
            //Spawn health
            //Spawn with spawnId
        }

        Rigidbody2D spawnItemRb = picksUpSpawnedWhenDead[spawnId].GetComponent<Rigidbody2D>();
        spawnItemRb.AddForce(Vector2.up * spawnForce);
    }
}
