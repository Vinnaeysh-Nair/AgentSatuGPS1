using UnityEngine;

public class BattleJetGun : MonoBehaviour
{
    [SerializeField] private Transform bullet;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float firerate;
    private float nextFireTime = 0f;
    
    private ObjectPooler pooler;
    void Start()
    {
        pooler = ObjectPooler.objPoolerInstance;
    }
    
    
    public void Shoot()
    {
        if (Time.time > nextFireTime)
        {
            pooler.SpawnFromPool(bullet.name, firePoint.position, firePoint.rotation);
            nextFireTime = Time.time + (1 / firerate);
        }
    }
}
