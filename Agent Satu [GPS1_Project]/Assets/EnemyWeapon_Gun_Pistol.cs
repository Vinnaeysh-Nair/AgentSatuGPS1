using System.Collections;
using UnityEngine;

public class EnemyWeapon_Gun_Pistol : EnemyWeapon_Guns
{
    public GameObject bullet;
    
    
    //Fields
    [SerializeField] private Transform[] firePoints;
    [SerializeField] private float shootInterval = .3f;
    private float nextFireTime = 0f;


    public bool detected = false;

    private bool canBurstShoot = true;

    void Start()
    {
        int size = transform.childCount;
        firePoints = new Transform[size];
        for (int i = 0; i < size; i++)
        {
            firePoints[i] = transform.GetChild(i);
        }
        
    }

    void Update()
    {
        //if (!detected) return;
        
        StartBurstShooting(5);
    }
    
    public void StartShooting()
    {
        if (Time.time > nextFireTime)
        {
            Shoot(bullet, firePoints);
            nextFireTime = Time.time + shootInterval;
        }
    }

    public void StartBurstShooting(int bulletPerBurst)
    {
        
        if (canBurstShoot)
        {
            canBurstShoot = false;
            print("shooting");

            for (int i = 0; i < bulletPerBurst; i++)
            {
                if (Time.time > nextFireTime)
                {
                    
                    Shoot(bullet, firePoints);
                    nextFireTime = Time.time + shootInterval;
                }

                if (i == bulletPerBurst)
                {
                    StartCoroutine(SetCanBurstShootToTrue());
                }

            }
        }
    }

    private IEnumerator SetCanBurstShootToTrue()
    {
        yield return new WaitForSeconds(3f);
        canBurstShoot = true;
    }
}
