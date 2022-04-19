using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;
    private PlayerWeapon[] playerWeapons;
    private PlayerInventory.Weapons[] _weaponsArray;
   
    
    public delegate void OnWeaponChange();
    public event OnWeaponChange onWeaponChangeDelegate;

    
    void Start()
    {
        GetPlayerWeaponsArray();
        _weaponsArray = GetComponent<PlayerInventory>().GetWeaponsArray();
        StartingWeapon();
    }


    void Update()
    {
        int prevSelectedWeapon = selectedWeapon;

        if (playerWeapons[selectedWeapon].GetReloading()) return;
     

        //Scroll wheel to change weapon
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (!IsNextWeaponUnlocked())
            {
                // int temp = FindNextUnlockedWeapon();
                // if (prevSelectedWeapon != temp)
                // {
                //     selectedWeapon = temp;
                //     StartingWeapon();
                //     WeaponIsSwitched();
                // }
                return;
            }
            
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
            {
                selectedWeapon++;
            }
            WeaponIsSwitched();
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (!IsPrevWeaponUnlocked())
            {
                // int temp = FindPrevUnlockedWeapon();
                // if (prevSelectedWeapon != temp)
                // {
                //     selectedWeapon = temp;
                //     print(temp);
                //     StartingWeapon();
                //     WeaponIsSwitched();
                // }
                return;
            }
            
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
            {
                selectedWeapon--;
            }
            WeaponIsSwitched();
        }
        
        //Changing through num keys
        for(int i=0;i<10;i++)
        {
            if(Input.GetKeyDown((KeyCode)(48+i)) && transform.childCount >= i)
            {
                //avoid getting null reference when pressing '0'
                if (i == 0) return;

                if (!IsSpecificWeaponUnlocked(i - 1)) return;
                selectedWeapon = i-1;
                WeaponIsSwitched();
            }
        }
        
        
        if (prevSelectedWeapon != selectedWeapon)
        {
            StartingWeapon();
        }
    }

    private void StartingWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            
            i++;
        }
    }

    
    
    private bool IsNextWeaponUnlocked()
    {
        int nextWep = 0;
        if (selectedWeapon + 1 < transform.childCount)
        {
            nextWep = selectedWeapon + 1;
        }
        else
        {
            nextWep = 0;
        }
        
        if (!_weaponsArray[nextWep].IsUnlocked) return false;
        return true;
    }

    private bool IsPrevWeaponUnlocked()
    {
        int prevWep = 0;
        if (selectedWeapon - 1 > 0)
        {
            prevWep = selectedWeapon - 1;
        }
        else
        {
            prevWep = transform.childCount - 1;
        }

        if (!_weaponsArray[prevWep].IsUnlocked) return false;
        return true;
    }

    private bool IsSpecificWeaponUnlocked(int key)
    {
        if (!_weaponsArray[key].IsUnlocked) return false;
        return true;
    }

    private void GetPlayerWeaponsArray()
    {
        playerWeapons = new PlayerWeapon[transform.childCount];
        
        for (int i = 0; i < transform.childCount; i++)
        { 
            playerWeapons[i] = transform.GetChild(i).GetComponent<PlayerWeapon>();
        }
    }

    // private int FindNextUnlockedWeapon()
    // {
    //     int foundWep = 0;
    //     for (int i = 0; i < _weaponsArray.Length; i++)
    //     {
    //         if (_weaponsArray[i].IsUnlocked && i != selectedWeapon)
    //         {
    //             foundWep = i;
    //             return foundWep;
    //         }
    //     }
    //     return 0;
    // }
    //
    // private int FindPrevUnlockedWeapon()
    // {
    //     int foundWep = 0;
    //     for (int i = _weaponsArray.Length - 1; i > 0; i--)
    //     {
    //         if (_weaponsArray[i].IsUnlocked)
    //         {
    //             foundWep = i;
    //             print(foundWep);
    //             return foundWep;
    //         }
    //     }
    //
    //     return foundWep;
    // }
    
    private void WeaponIsSwitched()
    {
        if (onWeaponChangeDelegate != null)
        {
            onWeaponChangeDelegate.Invoke();
        }
    }
}
