using UnityEngine;
using System.Collections;

public class EnemyWeapon_Guns : MonoBehaviour
{
    //Components
    [Header("Ref:")]
    [SerializeField] private Transform firePointContainer;
    [SerializeField] private Transform muzzleFlash;
    public GameObject bullet;
    
    
    private ObjectPooler pooler;
    private PauseMenu pauseMenu;
    

    //Fields
    [SerializeField] private float fireRate = 1f;
    // [SerializeField] private bool isBurst = false;  //for testing, can remove later
    // [SerializeField] private bool testShooting = false; //for testing, can remove later
    
    [Header("For burst weapons(only applies for StartBurstingShooting())")]
    [SerializeField] private float burstInterval = 5f;
    [SerializeField] private int bulletPerBurst;
    
    
    private Transform[] firePoints;
    private float nextFireTime = 0f;
    private bool canStartBurstShooting = true;
    private int shotBullet = 0;
    
    
    void Awake()
    {
        pooler = ObjectPooler.objPoolerInstance;
        pauseMenu = PauseMenu.Instance;
    }
    
    void Start()
    {
        int size = firePointContainer.childCount;
        firePoints = new Transform[size];
        for (int i = 0; i < size; i++)
        {
            firePoints[i] = firePointContainer.GetChild(i);
        }
    }


    
    //Call from enemy AI script, when player is detected
    //For single shot firing pattern (eg. pistol)
    public void StartShooting()
    {
        if (pauseMenu.gameIsPaused) return;
        
        if (Time.time > nextFireTime)
        {
            Shoot(bullet, firePoints);
            nextFireTime = Time.time + (1f / fireRate);
        }
    }
    
    //For burst firing pattern (eg. SMG)
    public void StartBurstShooting()
    {
        if (pauseMenu.gameIsPaused) return;
        
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
        muzzleFlash.gameObject.SetActive(true);
        StartCoroutine(SetMuzzleFlashInactive());

        
        foreach (Transform firePoint in firePoints)
        {
            pooler.SpawnFromPool(bullet.name, firePoint.position, firePoint.rotation);
        }
    }

    private IEnumerator SetMuzzleFlashInactive()
    {
        yield return new WaitForSeconds(.5f);
        muzzleFlash.gameObject.SetActive(false);
    }
}
