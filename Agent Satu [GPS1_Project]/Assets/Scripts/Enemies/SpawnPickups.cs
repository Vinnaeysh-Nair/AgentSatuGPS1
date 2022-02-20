using Cinemachine;
using UnityEngine;

public class SpawnPickups : MonoBehaviour
{
    //Components
    [Header("Max size is 2 (only health, or health and bullet pickup)")]
    [SerializeField] private Transform[] picksUpSpawnedWhenDead;
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
        
        //If index is 0, spawn health; if 1, spawn bullet pickup; 
        int itemIndex = 0;
        
        //If more than one element then only calculate chance
        if (picksUpSpawnedWhenDead.Length > 1)
        {
            if (rng <= bulletPickUpSpawnRate)
            {
                itemIndex = 1;
            }
        }


        //Spawning
        Vector2 enemyPos = transform.position;
        GameObject spawnedItem = pooler.SpawnFromPool(picksUpSpawnedWhenDead[itemIndex].name, new Vector2(enemyPos.x, enemyPos.y + spawnOffsetY), Quaternion.identity);

        //Physics effect
        Rigidbody2D spawnedItemRb = spawnedItem.GetComponent<Rigidbody2D>();
        spawnedItemRb.AddForce(new Vector2(Random.Range(-2f, 2f), 1 * spawnForce), ForceMode2D.Impulse);
    }
}
