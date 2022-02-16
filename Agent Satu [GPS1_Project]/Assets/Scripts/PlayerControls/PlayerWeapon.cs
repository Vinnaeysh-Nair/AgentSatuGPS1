using System.Collections;
using UnityEngine;


public class PlayerWeapon : MonoBehaviour
{
    //Components
    public GameObject bullet;
    public Transform firePoint;
    
    private ObjectPooler pooler;
    //public PlayerAnimationController animCon;
    
    
    //Fields
    [SerializeField] private float shootStanceDelay = 1f;

    void Awake()
    {
        pooler = ObjectPooler.objPoolerInstance;
    }
    
    void Update()
    {      
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Shoot());
          
        }
    }
    
    private IEnumerator Shoot()
    {
        GameObject shotBullet = pooler.SpawnFromPool(bullet.name, firePoint.position, firePoint.rotation);
        StartCoroutine(SetBulletInactive(shotBullet));
        //Instantiate(bullet, firePoint.position, firePoint.rotation);
        //animCon.OnShooting();

        //IEnumerator allows delay after a task
        yield return new WaitForSeconds(shootStanceDelay);
        //animCon.OnStopShooting();
    }
    
    //Become inactive after a duration after being fired. 
    private IEnumerator SetBulletInactive(GameObject shotBullet)
    {
        yield return new WaitForSeconds(1f);
        shotBullet.SetActive(false);
    }
}
