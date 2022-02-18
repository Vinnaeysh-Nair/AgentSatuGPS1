using System;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerWeapon : MonoBehaviour
{
    //Components
    public GameObject bullet;
    
    [Header("Shotgun uses extraFirePoint, the others use only firePoint and can ignore extra (put arr size 0)")]
    public Transform firePoint;
    public Transform[] extraFirePoint;
    private ObjectPooler pooler;
    //public PlayerAnimationController animCon;
    private PlayerInventory inventory;
    private List<PlayerInventory.Weapons> weaponsList;
    
    
    //Fields
    [Header("Settings")]
    [SerializeField] private int wepId;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float reloadTime = 1f;
    [SerializeField] private int clipSize;
    //[SerializeField] private float shootStanceDelay = 1f;
    [SerializeField] private bool isContinuousShooting = false;
    [SerializeField] private bool isMultishot = false;
    public bool isUnlocked = false;

    private float nextFireTime = 0f;
    private int currTotalAmmo;
    private int currAmmoReserve;
    private int currClip;
    private bool reloading = false;
    

    void Awake()
    {
        pooler = ObjectPooler.objPoolerInstance;
        
        inventory = GetComponentInParent<PlayerInventory>();
        weaponsList = inventory.GetWeaponsList();
    }

    void Start()
    {
        if (wepId == 0) return;
        
        //Initial setup
        currTotalAmmo = weaponsList[wepId - 1].GetTotalAmmo();
        if (currTotalAmmo >= clipSize)
        {
            currClip = clipSize;
            currAmmoReserve = currTotalAmmo - clipSize;
        }
        else
        {
            currClip = currTotalAmmo;
            currAmmoReserve = 0;
        }
    }
    
    private void OnEnable()
    {
        if (wepId == 0) return;

        //If theres a change in totalAmmo, update the reserve
        int prevTotalAmmo = currTotalAmmo;
        currTotalAmmo = weaponsList[wepId - 1].GetTotalAmmo();

        int diff = currTotalAmmo - prevTotalAmmo;
        if (diff > 0)
        {
            currAmmoReserve += diff;
        }
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
        if (HaveAmmo() && !ClipFull())
        {
            if (Input.GetButtonDown("Reload") || ClipEmpty())
            {
                StartCoroutine(Reload());
                return;
            }
        }
        
        //Check if clip is emptied too
        if (ClipEmpty())
        {
            return;
        }
        
        
        
        //Shooting type
        if (isContinuousShooting)
        {
            ContinuousShooting();
        }
        else
        {
            SingleClickShooting();
        }
    }
    
    private bool ClipEmpty()
    {
        if (currClip > 0) return false;
        return true;
    }
    
    private bool ClipFull()
    {
        if (currClip == clipSize) return true;
        return false;
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
        }
    }

    private void ContinuousShooting()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + (1f / fireRate);
            Shoot();
        }
    }
    
    private void Shoot()
    {
        DecreaseAmmo();

        GameObject shotBullet = pooler.SpawnFromPool(bullet.name, firePoint.position, firePoint.rotation);
        StartCoroutine(SetBulletInactive(shotBullet));
        
        
        //For shotgun
        if (!isMultishot) return;
        foreach (Transform point in extraFirePoint)
        {
            shotBullet = pooler.SpawnFromPool(bullet.name, point.position, point.rotation);
            StartCoroutine(SetBulletInactive(shotBullet));
        }
        
        
        
        //Instantiate(bullet, firePoint.position, firePoint.rotation);
        //animCon.OnShooting();

        //IEnumerator allows delay after a task
        //yield return new WaitForSeconds(shootStanceDelay);
        //animCon.OnStopShooting();
    }


    private void DecreaseAmmo()
    {
        if (wepId != 0)
        {
            currClip--;
            currTotalAmmo--;
            weaponsList[wepId - 1].SetTotalAmmo(currTotalAmmo);
        }
    }

    private IEnumerator Reload()
    {
        if (!reloading)
        {
            reloading = true;
            
            yield return new WaitForSeconds(reloadTime);

            //Check if reloadAmount exceeds reserve
            int reloadAmount = clipSize - currClip;
            if (reloadAmount > currAmmoReserve)
            {
                currClip = currAmmoReserve;
                currAmmoReserve = 0;
            }
            else
            {
                currClip += reloadAmount;   
                currAmmoReserve -= reloadAmount;
            }
            reloading = false;
        }
    }

    public void ReplenishAmmo(int replenishAmount)
    {
        currTotalAmmo += replenishAmount;
        currAmmoReserve += replenishAmount;
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
