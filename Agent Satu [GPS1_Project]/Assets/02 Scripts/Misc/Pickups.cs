using UnityEngine;

public class Pickups : MonoBehaviour
{
    //Components
    private PlayerInventory _playerInventory;
    private PlayerHpSystem _playerHp;
    //private List<PlayerInventory.Weapons> _weaponsList;
    private PlayerInventory.Weapons[] _weaponsArray;

    private DisplayUnlockedWeapon _displayUnlockedWeapon;

    //Fields
    public int pickupId;
    
    [Header("Applicable to both ammo and health")]
    public int replenishAmount;
    private bool _collected = false;
    private PlayerWeapon[] _playerWeapons;
    
    //Audio
    [Header("Audio")]
    [SerializeField] private AudioClip pickupSound;
    private AudioSource audSrc;
    


    //Text
    [Space] [Space]
    [Header("Each id and their corresponding effects, do not change the notes")]
    [TextArea(3,7)] [SerializeField] private string notes;


    void Start()
    {
        audSrc = GetComponent<AudioSource>();
        
        
        Transform playerBody = GameObject.FindGameObjectWithTag("PlayerBody").GetComponent<Transform>();

        _playerInventory = playerBody.Find("WeaponPivot/PlayerInventory").GetComponent<PlayerInventory>();
        _playerHp = playerBody.GetComponent<PlayerHpSystem>();
   
        _displayUnlockedWeapon = DisplayUnlockedWeapon.Instance;
        

        int size = _playerInventory.transform.childCount;
        _playerWeapons = new PlayerWeapon[size];
        for (int i = 0; i < size; i++)
        {
            _playerWeapons[i] = _playerInventory.transform.GetChild(i).GetComponent<PlayerWeapon>();
        }
        
        _weaponsArray = _playerInventory.GetWeaponsArray();
    }
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (_collected) return;
            _collected = true;
            
            TriggerEffect();
            gameObject.SetActive(false);
            
            
            
            _collected = false;
        }
    }

    private void TriggerEffect()
    {
        if (pickupId == 0)
        {
            _playerHp.ReplenishHealth(replenishAmount);
        }
        else
        {
            TriggerReplenishAmmo();
        }
      
    }

    private void TriggerReplenishAmmo()
    {
        GameObject activeWeapon = _playerInventory.transform.GetChild(pickupId).gameObject;
        
        if (activeWeapon.gameObject.activeInHierarchy)
        {
            PlayerWeapon wep = activeWeapon.GetComponent<PlayerWeapon>();
            wep.ReplenishAmmo(replenishAmount);
        }
        
        UnlockWeapon(pickupId);
        
        //Update inventory
        int currTotal = _weaponsArray[pickupId].TotalAmmo;
        _weaponsArray[pickupId].TotalAmmo = currTotal + replenishAmount;   
    }

    private void UnlockWeapon(int wepId)
    {
        if (_playerWeapons[wepId].isUnlocked) return;
        
        _playerWeapons[wepId].isUnlocked = true;
        
        //Update inventory
        _playerInventory.GetWeapon(wepId).IsUnlocked = true;  
    
        
        _displayUnlockedWeapon.DisplayWeapon(wepId);
    }
}
