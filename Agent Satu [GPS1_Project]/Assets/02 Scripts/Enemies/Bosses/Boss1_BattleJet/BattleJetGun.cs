using System.Collections;
using UnityEngine;
using System;


public class BattleJetGun : MonoBehaviour
{
    [SerializeField] private TargetPointMovement targetPointMovement;
    [SerializeField] private Transform bullet;
    [SerializeField] private Transform firePoint;
    private float nextFireTime = 0f;

    private int firedShots = 0;
    [SerializeField] private bool canShoot = true;
    [SerializeField] private bool canBurstShoot = true;
   
    private ObjectPooler pooler;

    public event EventHandler OnFiredAllShots;

  
    void OnDestroy()
    {
        targetPointMovement.OnReachingIdle -= TargetPointMovement_OnReachingIdle;
       
    }
    void Start()
    {
        pooler = ObjectPooler.objPoolerInstance;
        targetPointMovement.OnReachingIdle += TargetPointMovement_OnReachingIdle;
        
    }
    
    public void Shoot(int shotsPerBurst, float firerate)
    {
        if (Time.time > nextFireTime)
        {
            if (!canShoot) return;

           
            pooler.SpawnFromPool(bullet.name, firePoint.position, firePoint.rotation);
            nextFireTime = Time.time + (1 / firerate);

            firedShots++;

            if (firedShots == shotsPerBurst)
            {
                canShoot = false;
                OnFiredAllShots?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    
    public void BurstShoot(int shotsPerBurst, float firerate, float timeUntilNextBurst)
    {
        if (Time.time > nextFireTime)
        {
            if (!canBurstShoot) return;

            pooler.SpawnFromPool(bullet.name, firePoint.position, firePoint.rotation);
            nextFireTime = Time.time + (1 / firerate);

            firedShots++;
            
            if (firedShots == shotsPerBurst)
            {
                canBurstShoot = false;
                OnFiredAllShots?.Invoke(this, EventArgs.Empty);
                
                StartCoroutine(SetCanBurstShootToTrue(timeUntilNextBurst));
            }
        }
    }
    
    
    
    private IEnumerator SetCanBurstShootToTrue(float timeUntilNextBurst)
    {
        yield return new WaitForSeconds(timeUntilNextBurst);
        
        firedShots = 0;
        canBurstShoot = true;
    }

    
    private void TargetPointMovement_OnReachingIdle(object sender, System.EventArgs e)
    {
        firedShots = 0;
        canShoot = true;
    }
}
