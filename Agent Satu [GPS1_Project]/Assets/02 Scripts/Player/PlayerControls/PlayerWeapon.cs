using System.Collections;
using UnityEngine;
using System;


public class PlayerWeapon : MonoBehaviour
{
    //Components
    [Header("Ref: ")]
    [SerializeField] private Transform bullet;
    [SerializeField] private Transform firePointContainer;
    [SerializeField] private Transform muzzleFlash;
    
    
    [Header("Sound")] 
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;
    
    private SoundManager _soundManager;
    private AudioSource _reloadSource;

    
    private Transform[] firePoints;
    private ObjectPooler pooler;
    private PlayerInventory inventory;
    private PlayerInventory.Weapons[] weaponsArray;

    
    //UI
    private PauseMenu pauseMenu;
    
    //Fields
    [Header("Settings")]
    [SerializeField] private int wepId;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float reloadTime = 1f;
    [SerializeField] private int clipSize;
    [SerializeField] private bool isContinuousShooting = false;

    //Ammo counts
    private float nextFireTime = 0f;
    private int currTotalAmmo;
    private int currAmmoReserve;
    private int currClip;
    
    private bool reloading = false;
    private Coroutine reloadRoutine;

    
    
    public static event Action<int, int, int> OnAmmoUpdate;



    public bool GetReloading()
    {
        return reloading;
    }
    
    
    void Awake()
    {
        pooler = ObjectPooler.objPoolerInstance;
        pauseMenu = PauseMenu.Instance;
 
        
        inventory = GetComponentInParent<PlayerInventory>();
        weaponsArray = inventory.GetWeaponsArray();
    }

    void OnDisable()
    {
        if (reloadRoutine != null)
            StopCoroutine(reloadRoutine);
        
        if (_reloadSource != null)
            _reloadSource.Stop();
        
        reloading = false;
    }


    void OnEnable()
    {
        UpdateAmmoDisplay();

        if (muzzleFlash.gameObject.activeSelf)
        {
            muzzleFlash.gameObject.SetActive(false);
        }
        
        
        if (wepId == 0) return;

        //If theres a change in totalAmmo, update the reserve
        int prevTotalAmmo = currTotalAmmo;
        currTotalAmmo = weaponsArray[wepId].TotalAmmo;

        int diff = currTotalAmmo - prevTotalAmmo;
        if (diff > 0)
        {
            currAmmoReserve += diff;
        }
    }


    void Start()
    {
        _soundManager = SoundManager.Instance;

        
        //Get firepoints
        int size = firePointContainer.childCount;
        firePoints = new Transform[size];
        for (int i = 0; i < size; i++)
        {
            firePoints[i] = firePointContainer.GetChild(i).transform;
        }
        
        //If pistol, ignore setting up ammo
        if (wepId != 0)
        {
            //Initial setup
            currTotalAmmo = weaponsArray[wepId].TotalAmmo;
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
        
        UpdateAmmoDisplay();
    }


    
    void Update()
    {
        if (pauseMenu.gameIsPaused) return;
        
        if (wepId == 0)
        {
            if (isContinuousShooting)
            {
                ContinuousShooting();
            }
            else
            {
                SingleClickShooting();
            }
            return;
        }
        
        
        
        //Below are guns that need to check for reloading
        
        //If need to reload and have ammo
        if (HaveAmmo() && !ClipFull())
        {
            if (Input.GetButtonDown("Reload") || ClipEmpty())
            {
                reloadRoutine = StartCoroutine(Reload());
                return;
            }
        }
        
        //Check if clip is emptied too
        if (ClipEmpty())
        {
            return;
        }

        if (reloading) return;
        
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
        //Sound
        _soundManager.PlayEffect(shootSound, true);
        
        //Muzzle flash
        muzzleFlash.gameObject.SetActive(true);
        StartCoroutine(SetMuzzleFlashInactive());
        
        
        DecreaseAmmo();

        foreach (Transform firePoint in firePoints)
        {
            GameObject shotBullet = pooler.SpawnFromPool(bullet.name, firePoint.position, firePoint.rotation);
            //StartCoroutine(SetBulletInactive(shotBullet));
        }

        UpdateAmmoDisplay();
    }


    private void DecreaseAmmo()
    {
        if (wepId != 0)
        {
            currClip--;
            currTotalAmmo--;
            weaponsArray[wepId].TotalAmmo = currTotalAmmo;
        }
    }
    
    private IEnumerator Reload()
    {
        if (!reloading)
        {
            reloading = true;
            
            _reloadSource = _soundManager.PlayEffect(reloadSound);
            yield return new WaitForSeconds(reloadTime);

         
            //Check if reloadAmount exceeds reserve
            int reloadAmount = clipSize - currClip;
            if (reloadAmount > currAmmoReserve)     
            {
                if (currAmmoReserve > clipSize)     //if reserve have enough
                {
                    currClip += reloadAmount;
                }
                else
                {
                    currClip += currAmmoReserve;
                }
            
                currAmmoReserve = 0;
            }
            else
            {
                currClip += reloadAmount;
                currAmmoReserve -= reloadAmount;
            }

            UpdateAmmoDisplay();
            reloading = false;
        }
    }

    public void UpdateAmmoDisplay()
    {
        // if (onAmmoUpdateDelegate != null)
        // {
        //     onAmmoUpdateDelegate.Invoke();
        // }
        if(OnAmmoUpdate != null) OnAmmoUpdate.Invoke(wepId, currClip, currAmmoReserve);
    }

    //For pickup items
    public void ReplenishAmmo(int replenishAmount)
    {
        currTotalAmmo += replenishAmount;
        currAmmoReserve += replenishAmount;

        UpdateAmmoDisplay();
    }

   
    private void WeaponSwitching_OnWeaponChange()
    {
        UpdateAmmoDisplay();
    }


    private IEnumerator SetMuzzleFlashInactive()
    {
        yield return new WaitForSeconds(.5f);
        muzzleFlash.gameObject.SetActive(false);
    }
}
