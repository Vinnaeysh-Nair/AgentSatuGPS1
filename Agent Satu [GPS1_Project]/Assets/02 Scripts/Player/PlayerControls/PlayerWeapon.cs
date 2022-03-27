using System.Collections;
using UnityEngine;



public class PlayerWeapon : MonoBehaviour
{
    //Components
    [SerializeField] private Transform bullet;
    [SerializeField] private Transform firePointContainer;

    [Header("Audio")] 
    [SerializeField] private AudioClip audClip;
    private AudioSource audsrc;


    
    private Transform[] firePoints;
    private ObjectPooler pooler;
    private PlayerInventory inventory;
    //private List<PlayerInventory.Weapons> weaponsList;
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
    public bool isUnlocked = false;

    //Ammo counts
    private float nextFireTime = 0f;
    private int currTotalAmmo;
    private int currAmmoReserve;
    private int currClip;
    private bool reloading = false;

    public delegate void OnAmmoUpdate();
    public event OnAmmoUpdate onAmmoUpdateDelegate;

    public int WepId
    {
        get => wepId;
    }

    public int CurrClip
    {
        get => currClip;
    }

    public int CurrReserve
    {
        get => currAmmoReserve;
    }


    public bool GetReloading()
    {
        return reloading;
    }

    private void OnDestroy()
    {
        WeaponSwitching wepSwitch = transform.parent.GetComponent<WeaponSwitching>();
        wepSwitch.onWeaponChangeDelegate += WeaponSwitching_OnWeaponChange;
    }
    
    void Awake()
    {
        pooler = ObjectPooler.objPoolerInstance;
        pauseMenu = PauseMenu.Instance;

        inventory = GetComponentInParent<PlayerInventory>();
        weaponsArray = inventory.GetWeaponsArray();
        
        isUnlocked =  weaponsArray[wepId].IsUnlocked;
    }

    


    void Start()
    {
        WeaponSwitching wepSwitch = transform.parent.GetComponent<WeaponSwitching>();
        wepSwitch.onWeaponChangeDelegate += WeaponSwitching_OnWeaponChange;
        
        //get unlocked status from inventory

        audsrc = GetComponent<AudioSource>();
        
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


    private void OnEnable()
    {
        UpdateAmmoDisplay();
        
        
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
        audsrc.PlayOneShot(audClip);
        
        DecreaseAmmo();

        foreach (Transform firePoint in firePoints)
        {
            GameObject shotBullet = pooler.SpawnFromPool(bullet.name, firePoint.position, firePoint.rotation);
            StartCoroutine(SetBulletInactive(shotBullet));
        }

        UpdateAmmoDisplay();
        
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
            weaponsArray[wepId].TotalAmmo = currTotalAmmo;
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

            UpdateAmmoDisplay();
            reloading = false;
        }
    }

    private void UpdateAmmoDisplay()
    {
        if (onAmmoUpdateDelegate != null)
        {
            onAmmoUpdateDelegate.Invoke();
        }
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
    
    //Become inactive after a duration after being fired. 
    private IEnumerator SetBulletInactive(GameObject shotBullet)
    {
        yield return new WaitForSeconds(1f);
        shotBullet.SetActive(false);
    }
}
