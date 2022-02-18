using UnityEngine;

public class SpawnPickups : MonoBehaviour
{
    //Components
    [SerializeField] Transform[] picksUpSpawnedWhenDead = new Transform[2];
    [SerializeField] [Range(0f, 1f)] private float bulletSpawnRate = 0.6f;
    [SerializeField] private float spawnForce = 1f;

    private ObjectPooler pooler;

    void Awake()
    {
        enabled = false;
        
        pooler = ObjectPooler.objPoolerInstance;
    }
    void OnEnable()
    {
        SpawnItem();
    }
    
    private void SpawnItem()
    { 
        //Determine spawn health/pickups/none
        float rng = Random.Range(0f, 1f);
        int itemIndex = 0;

        if (rng > bulletSpawnRate)
        { 
            //Spawn health
            Debug.Log("spawned health");
            itemIndex = 1;
        }
        else
        {
            //Spawn bullet pickup
            Debug.Log("spawned bullet pickup");
        }
       
        Debug.Log(transform.position);
        GameObject spawnedItem = pooler.SpawnFromPool(picksUpSpawnedWhenDead[itemIndex].name, transform.position, transform.rotation);

        Rigidbody2D spawnedItemRb = spawnedItem.GetComponent<Rigidbody2D>();
        spawnedItemRb.AddForce(Vector2.up * spawnForce);
        
    }
}
