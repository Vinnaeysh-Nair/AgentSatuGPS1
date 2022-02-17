using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Weapons[] weaponsArray;

    public Weapons[] GetWeaponsArray()
    {
        return weaponsArray;
    }
    
    
    [System.Serializable]
    public class Weapons
    {
        [SerializeField] private int weaponId;
        [SerializeField] private int totalAmmo;

        //Getter
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
