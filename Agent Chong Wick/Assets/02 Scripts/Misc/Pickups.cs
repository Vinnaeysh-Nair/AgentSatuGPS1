using UnityEngine;

public class Pickups : MonoBehaviour
{
    //Components
    private PlayerInventory _playerInventory;
    private PlayerHpSystem _playerHp;
    private PlayerInventory.Weapons[] _weaponsArray;

    private DisplayUnlockedWeapon _displayUnlockedWeapon;

    //Fields
    public int pickupId;
    
    [Header("Applicable to both ammo and health")]
    public int replenishAmount;
    private bool _collected = false;
    private PlayerWeapon[] _playerWeapons;

    //Sound
    private SoundManager _soundManager;
    
    [Header("Sound")]
    [SerializeField] private AudioClip pickUpSound;



    //Text
    [Space] [Space]
    [Header("Each id and their corresponding effects, do not change the notes")]
    [TextArea(3,7)] [SerializeField] private string notes;


    void Start()
    {
        //SoundManager
        _soundManager = SoundManager.Instance;
        

        PlayerMain playerMain = PlayerMain.Instance;
        Transform playerBody = playerMain.transform;
        
        _playerInventory = playerBody.Find("WeaponPivot/PlayerInventory").GetComponent<PlayerInventory>();
        _playerHp = playerMain.PlayerHpSystem;
   
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

            _soundManager.PlayEffect(pickUpSound, true);
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
        if (_weaponsArray[wepId].IsUnlocked) return;
        
       _weaponsArray[wepId].IsUnlocked = true;
       _displayUnlockedWeapon.DisplayWeapon(wepId);
    }
}
