using UnityEngine;
using System.Collections;

public class EnemyWeapon_Guns : MonoBehaviour
{
    //Components
    private ObjectPooler pooler;
    public GameObject bullet;


    //Fields
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private bool isBurst = false;  //for testing, can remove later
    [SerializeField] private bool testShooting = false; //for testing, can remove later
    
    [Header("For burst weapons(tick isBurst for stats below to apply)")]
    [SerializeField] private float burstInterval = 5f;
    [SerializeField] private int bulletPerBurst;

    private Transform[] firePoints;
    private float nextFireTime = 0f;
    private bool canStartBurstShooting = true;
    private int shotBullet = 0;
    
    void Awake()
    {
        pooler = ObjectPooler.objPoolerInstance;
    }
    
    void Start()
    {
        int size = transform.childCount;
        firePoints = new Transform[size];
        for (int i = 0; i < size; i++)
        {
            firePoints[i] = transform.GetChild(i);
        }
    }
    
    //For testing
    void Update()
    {
        if (!testShooting) return;
        if (isBurst)
        {
            StartBurstShooting();
        }
        else
        {
            StartShooting();
        }
    }
    
    
    //Call from enemy AI script, when player is detected
    //For single shot firing pattern (eg. pistol)
    public void StartShooting()
    {
        if (Time.time > nextFireTime)
        {
            Shoot(bullet, firePoints);
            nextFireTime = Time.time + (1f / fireRate);
        }
    }
    
    //For burst firing pattern (eg. SMG)
    public void StartBurstShooting()
    {
        if (canStartBurstShooting && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + (1f / fireRate);
            Shoot(bullet, firePoints);
            shotBullet++;
        }

        if (shotBullet == bulletPerBurst)
        {
            StartCoroutine(SetCanStartBurstShootingToTrue());
            canStartBurstShooting = false;
            shotBullet = 0;
        }
    }


    private IEnumerator SetCanStartBurstShootingToTrue()
    {
        yield return new WaitForSeconds(burstInterval);
        canStartBurstShooting = true;
    }
    
    private void Shoot(GameObject bullet, Transform[] firePoints)
    {
        foreach (Transform firePoint in firePoints)
        {
            GameObject shotBullet = pooler.SpawnFromPool(bullet.name, firePoint.position, firePoint.rotation);
            StartCoroutine(SetBulletInactive(shotBullet));
        }
    }

    
    //Become inactive after a duration after being fired. 
    private IEnumerator SetBulletInactive(GameObject shotBullet)
    {
        yield return new WaitForSeconds(1f);
        shotBullet.SetActive(false);
    }

}
