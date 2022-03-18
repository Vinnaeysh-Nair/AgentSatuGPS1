using UnityEngine;
using System;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;
    private PlayerWeapon[] playerWeapons;
    //public event EventHandler OnWeaponChange;

    public delegate void OnWeaponChange();
    public event OnWeaponChange onWeaponChangeDelegate;
    
    void Start()
    {
        GetPlayerWeaponsArray();
        StartingWeapon();
    }

    
    void Update()
    {
        int prevSelectedWeapon = selectedWeapon;

        if (playerWeapons[selectedWeapon].GetReloading()) return;

        //Scroll wheel to change weapon
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (!IsNextWeaponUnlocked()) return;
            
            
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
            {
                selectedWeapon++;
                WeaponIsSwitched();
            }
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (!IsPrevWeaponUnlocked()) return;
            
            
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
            {
                selectedWeapon--;
                WeaponIsSwitched();
            }
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

    private void WeaponIsSwitched()
    {
        if (onWeaponChangeDelegate != null)
        {
            onWeaponChangeDelegate.Invoke();
        }
    }
}
