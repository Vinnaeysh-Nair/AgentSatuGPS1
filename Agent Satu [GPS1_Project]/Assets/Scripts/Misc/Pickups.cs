using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    //Components
    private PlayerInventory playerInventory;
    private PlayerHpSystem playerHp;
    private List<PlayerInventory.Weapons> weaponsList;

    //Fields
    public int pickupId;
    
    [Header("Applicable to both ammo and health")]
    public int replenishAmount;

    private bool collected = false;
    private PlayerWeapon[] playerWeapons;

    //Text
    [Space] [Space]
    [Header("Each id and their corresponding effects, do not change the notes")]
    [TextArea(3,7)] [SerializeField] private string notes;
    
    void Start()
    {
        playerInventory = transform.Find("/Player/Pivot/Arms/PlayerInventory").GetComponent<PlayerInventory>();
        playerWeapons = playerInventory.GetComponentsInChildren<PlayerWeapon>();
        playerHp = transform.Find("/Player/PlayerBody").GetComponent<PlayerHpSystem>();
        
        
        weaponsList = playerInventory.GetWeaponsList();
    }
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (collected) return;
            
            collected = true;
            TriggerEffect();
            gameObject.SetActive(false);
            collected = false;
        }
    }

    private void TriggerEffect()
    {
        if (pickupId == 0)
        {
            playerHp.ReplenishHealth(replenishAmount);
        }
        else
        {
            TriggerReplenishAmmo();
        }
    }

    private void TriggerReplenishAmmo()
    {
        GameObject activeWeapon = playerInventory.transform.GetChild(pickupId).gameObject;
        
        if (activeWeapon.gameObject.activeInHierarchy)
        {
            PlayerWeapon wep = activeWeapon.GetComponent<PlayerWeapon>();
            wep.ReplenishAmmo(replenishAmount);
        }
        
        UnlockWeapon(pickupId);
        
        //Update inventory
        int currTotal = weaponsList[pickupId - 1].GetTotalAmmo();
        weaponsList[pickupId - 1].SetTotalAmmo(currTotal + replenishAmount);   
    }

    private void UnlockWeapon(int wepId)
    {
        if (playerWeapons[wepId].isUnlocked) return;
        playerWeapons[wepId].isUnlocked = true;
    }
}
