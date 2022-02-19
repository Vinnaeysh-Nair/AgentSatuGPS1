using UnityEngine;
using System.Collections;

public class EnemyWeapon_Guns : MonoBehaviour
{
    //Components
    private ObjectPooler pooler;

    

    void Awake()
    {
        pooler = ObjectPooler.objPoolerInstance;
    }


    public void Shoot(GameObject bullet, Transform[] firePoints)
    {
        foreach (Transform firePoint in firePoints)
        {
            GameObject shotBullet = pooler.SpawnFromPool(bullet.name, firePoint.position, firePoint.rotation);
            StartCoroutine(SetBulletInactive(shotBullet));
        }
    }

    public IEnumerator BurstShoot(GameObject bullet, Transform[] firePoints, float fireRate)
    {
        yield return new WaitForSeconds(fireRate);
        
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
