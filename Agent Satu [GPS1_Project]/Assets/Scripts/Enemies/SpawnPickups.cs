using Cinemachine;
using UnityEngine;

public class SpawnPickups : MonoBehaviour
{
    //Components
    [SerializeField] Transform[] picksUpSpawnedWhenDead = new Transform[2];
    [SerializeField] [Range(0f, 1f)] private float bulletPickUpSpawnRate = 0.6f;
    [SerializeField] private float spawnForce = 1f;
    [SerializeField] private float spawnOffsetY = 6f;

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

        if (rng > bulletPickUpSpawnRate)
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
       
        
        Vector2 enemyPos = transform.position;
        GameObject spawnedItem = pooler.SpawnFromPool(picksUpSpawnedWhenDead[itemIndex].name, new Vector2(enemyPos.x, enemyPos.y + spawnOffsetY), Quaternion.identity);

        Rigidbody2D spawnedItemRb = spawnedItem.GetComponent<Rigidbody2D>();
        spawnedItemRb.AddForce(new Vector2(Random.Range(-2f, 2f), 1 * spawnForce), ForceMode2D.Impulse);
    }
}
