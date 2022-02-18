using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;
    private PlayerWeapon[] playerWeapons;
    
    void Start()
    {
        GetPlayerWeaponsArray();
        StartingWeapon();
    }

    
    void Update()
    {
        int prevSelectedWeapon = selectedWeapon;
        

        //Scroll wheel to change weapon
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (!IsNextWeaponUnlocked()) return;
            
            
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
            
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (!IsPrevWeaponUnlocked()) return;
            
            
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
            
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
            }
        }
        
        
        if (prevSelectedWeapon != selectedWeapon)
        {
            StartingWeapon();
        }
    }

    void StartingWeapon()
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
        
        if (!playerWeapons[nextWep].isUnlocked) return false;
        return true;
    }

    private bool IsPrevWeaponUnlocked()
    {
        int prevWep = 0;
        if (selectedWeapon - 1 >= 0)
        {
            prevWep = selectedWeapon - 1;
        }
        else
        {
            prevWep = transform.childCount - 1;
        }

        if (!playerWeapons[prevWep].isUnlocked) return false;
        return true;
    }

    private bool IsSpecificWeaponUnlocked(int key)
    {
        if (!playerWeapons[key].isUnlocked) return false;
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
}
