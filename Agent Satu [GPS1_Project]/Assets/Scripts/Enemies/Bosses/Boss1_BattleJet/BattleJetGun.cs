using System.Collections;
using UnityEngine;

public class BattleJetGun : MonoBehaviour
{
    [SerializeField] private TargetPointMovement targetPointMovement;
    [SerializeField] private Transform bullet;
    [SerializeField] private Transform firePoint;
    //[SerializeField] private float firerate;
    private float nextFireTime = 0f;

    private int firedShots = 0;
    [SerializeField]private bool canShoot = true;
    
    private ObjectPooler pooler;
   
    void OnDestroy()
    {
        targetPointMovement.OnReachingTarget -= TargetPointMovement_OnReachingTarget;
    }
    void Start()
    {
        pooler = ObjectPooler.objPoolerInstance;
        targetPointMovement.OnReachingTarget += TargetPointMovement_OnReachingTarget;
    }
    
    
    public void BurstShoot(int shotsPerBurst, float firerate, float timeUntilNextBurst)
    {
        if (Time.time > nextFireTime)
        {
            if (!canShoot) return;
            print("shooting");
            
            pooler.SpawnFromPool(bullet.name, firePoint.position, firePoint.rotation);
            nextFireTime = Time.time + (1 / firerate);

            firedShots++;

            if (firedShots == shotsPerBurst)
            {
                canShoot = false;
                StartCoroutine(SetCanShootToFalse(timeUntilNextBurst));
            }
        }
    }
    
    public void Shoot(int shotsPerBurst, float firerate)
    {
        if (Time.time > nextFireTime)
        {
            if (!canShoot) return;
            print("shooting");
            pooler.SpawnFromPool(bullet.name, firePoint.position, firePoint.rotation);
            nextFireTime = Time.time + (1 / firerate);

            firedShots++;

            if (firedShots == shotsPerBurst)
            {
                canShoot = false;
            }
        }
    }

    private IEnumerator SetCanShootToFalse(float timeUntilNextBurst)
    {
        yield return new WaitForSeconds(timeUntilNextBurst);
        
        firedShots = 0;
        canShoot = true;
    }

    private void TargetPointMovement_OnReachingTarget(object sender, System.EventArgs e)
    {
        firedShots = 0;
        canShoot = true;
    }
}
