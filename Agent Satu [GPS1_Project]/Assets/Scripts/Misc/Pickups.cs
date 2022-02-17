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

    //Text
    [Space] [Space]
    [Header("Each id and their corresponding effects, do not change the notes")]
    [TextArea(3,7)] [SerializeField] private string notes;
    
    void Start()
    {
        playerInventory = transform.Find("/Player/Pivot/Arms/PlayerInventory").GetComponent<PlayerInventory>();
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
            Destroy(gameObject);
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
        
        //Update inventory
        int currTotal = weaponsList[pickupId - 1].GetTotalAmmo();
        weaponsList[pickupId - 1].SetTotalAmmo(currTotal + replenishAmount);   
    }
}
