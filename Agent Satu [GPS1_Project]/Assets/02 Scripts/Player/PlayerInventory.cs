using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private PlayerWeaponSaveSO playerWeaponSaveSo;

    [Header("Array elements corresponds to order of gun in the PlayerInventory object (Pistol is excluded)")]
    [SerializeField] private  Weapons[] weaponsArray;


    [System.Serializable]
    public class Weapons
    {
        public string name;

        
        [SerializeField] private int weaponId;
        [SerializeField] private int totalAmmo;
        [SerializeField] private bool isUnlocked;

        //Getter
        public int WeaponId
        {
            get => weaponId;
            set => weaponId = value;
        }

        public bool IsUnlocked
        {
            get => isUnlocked;
            set => isUnlocked = value;
        }

        public int TotalAmmo
        {
            get => totalAmmo;
            set => totalAmmo = value;
        }
    }


    public Weapons[] GetWeaponsArray()
    {
        return weaponsArray;
    }
    
    private void OnDestroy()
    {
        TransitionScript.OnChangeLevel -= TransitionScript_OnChangeLevelDelegate;
    }

    private void Awake()
    {
        TransitionScript.OnChangeLevel += TransitionScript_OnChangeLevelDelegate;
        RetrieveGunState();
    }
    
    private void TransitionScript_OnChangeLevelDelegate()
    {
        SaveGunState();
    }

    private void SaveGunState()
    {
        print("saving");
        
        ChangeGunState(weaponsArray, playerWeaponSaveSo.savedWepState);
    }
    
    private void RetrieveGunState()
    {
        print("retrieving");
   
        ChangeGunState(playerWeaponSaveSo.savedWepState, weaponsArray);
    }

    
    private void ChangeGunState(Weapons[] source, Weapons[] destination)
    {
        int size = weaponsArray.Length;
        for (int i = 0; i < size; i++)
        {
            Weapons tempSource = source[i];
            Weapons tempDest = destination[i];
           
            
            tempDest.name = tempSource.name;
            tempDest.WeaponId = tempSource.WeaponId;
            tempDest.TotalAmmo = tempSource.TotalAmmo;
            tempDest.IsUnlocked = tempSource.IsUnlocked;
        }
    }


    public void ResetGunState()
    {
        Weapons[] reset = playerWeaponSaveSo.resetWepState;
        
        ChangeGunState(reset, weaponsArray);
        ChangeGunState(reset, playerWeaponSaveSo.savedWepState);
    }

    
    public void PrintInventory()
    {
        foreach (Weapons wep in weaponsArray)
        {
            print($"Name: {wep.name}, Id: {wep.WeaponId}, IsUnlocked: {wep.IsUnlocked}");
        }
    }

    public void PrintWep(int id)
    {
        print($"Name: {weaponsArray[id].name}, Id: {weaponsArray[id].WeaponId}, IsUnlocked: {weaponsArray[id].IsUnlocked}");
    }
}
