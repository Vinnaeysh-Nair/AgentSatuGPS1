using System.Collections;
using UnityEngine;


public class PlayerWeapon : MonoBehaviour
{
    //Components
    public GameObject bullet;
    public Transform firePoint;
    private ObjectPooler pooler;
    //public PlayerAnimationController animCon;
    private PlayerInventory inventory;
    private PlayerInventory.Weapons[] weaponsArray;
    
    
    //Fields
    [Header("Settings")]
    [SerializeField] private int wepId;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float reloadTime = 1f;
    [SerializeField] private int clipSize;
    [SerializeField] private float shootStanceDelay = 1f;

    private float nextFireTime = 0f;
    private int currTotalAmmo;
    private int currAmmoReserve;
    private int currClip;
    private bool reloading = false;
    

    void Awake()
    {
        pooler = ObjectPooler.objPoolerInstance;
        
        inventory = GetComponentInParent<PlayerInventory>();
        weaponsArray = inventory.GetWeaponsArray();
    }

    void Start()
    {
        currClip = clipSize;

        if (wepId == 0) return;
        currTotalAmmo = weaponsArray[wepId - 1].GetTotalAmmo();
        currAmmoReserve = currTotalAmmo - clipSize;
    }
    void Update()
    {
        if (wepId == 0)
        {
            SingleClickShooting();
            return;
        }
        
        
        //Below are guns that need to check for reloading
        
        //If need to reload and have ammo
        if (ClipEmpty() && HaveAmmo())
        {
            StartCoroutine(Reload());
            return;
        }

        //Check if clip is emptied too
        if (ClipEmpty())
        {
            return;
        }
        
        //Shooting 
        if (wepId == 1)
        {
            ContinuousShooting();
            return;
        }

        if (wepId == 2)
        {
            SingleClickShooting();
            return;
        }
    }

    private bool ClipEmpty()
    {
        if (currClip > 0) return false;
        return true;
    }
    private bool HaveAmmo()
    {
        if (currAmmoReserve > 0) return true;
        return false;
    }
    
    private void SingleClickShooting()
    {
        if (Input.GetButtonDown("Fire1")  && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + (1f / fireRate);
            Shoot();
            PrintAmmo();
        }
    }

    private void ContinuousShooting()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + (1f / fireRate);
            Shoot();
            PrintAmmo();
        }
    }
    
    private void Shoot()
    {
        if (wepId != 0)
        {
            currClip--;
            currTotalAmmo--;
            weaponsArray[wepId - 1].SetTotalAmmo(currTotalAmmo);
        }
        GameObject shotBullet = pooler.SpawnFromPool(bullet.name, firePoint.position, firePoint.rotation);
        StartCoroutine(SetBulletInactive(shotBullet));
        
        
        //Instantiate(bullet, firePoint.position, firePoint.rotation);
        //animCon.OnShooting();

        //IEnumerator allows delay after a task
        //yield return new WaitForSeconds(shootStanceDelay);
        //animCon.OnStopShooting();
    }

    private IEnumerator Reload()
    {
        if (!reloading)
        {
            reloading = true;
            
            yield return new WaitForSeconds(reloadTime);
            currClip = clipSize;
            currAmmoReserve -= clipSize;
            
            reloading = false;
        }
    }
    
    
    //Become inactive after a duration after being fired. 
    private IEnumerator SetBulletInactive(GameObject shotBullet)
    {
        yield return new WaitForSeconds(1f);
        shotBullet.SetActive(false);
    }

    private void PrintAmmo()
    {
        Debug.Log(currClip + "/" + currAmmoReserve);
    }
}
