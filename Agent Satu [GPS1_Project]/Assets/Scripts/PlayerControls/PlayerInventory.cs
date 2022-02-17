using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Array elements corresponds to order of gun in the PlayerInventory object (Pistol is excluded)")]
    [SerializeField] private List<Weapons> weaponsList;

    public List<Weapons> GetWeaponsList()
    {
        return weaponsList;
    }
    
    
    [System.Serializable]
    public class Weapons
    {
        [Header("Id put (Element number + 1), Total ammo free to change in anyway")]
        [SerializeField] private int weaponId;
        [SerializeField] private int totalAmmo;

        //Getter
        public int GetWeaponId()
        {
            return weaponId;
        }
        public int GetTotalAmmo()
        {
            return totalAmmo;
        }
        
        //Setter
        public void SetTotalAmmo(int totalAmmo)
        {
            this.totalAmmo = totalAmmo;
        }

    }
}
