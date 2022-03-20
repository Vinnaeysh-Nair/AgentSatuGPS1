using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private PlayerSaveSO playerSaveSo;

    [Header("Array elements corresponds to order of gun in the PlayerInventory object (Pistol is excluded)")]
    [SerializeField] private Weapons[] weaponsArray;
  
    [System.Serializable]
    public class Weapons
    {
        [Header("Id put (Element number + 1), Total ammo free to change in anyway")] 
        public string name;
        
        [SerializeField] private int weaponId;
        [SerializeField] private int totalAmmo;
        [SerializeField] private bool isUnlocked;

        //Getter
        public int WeaponId { get; set; }
        
    

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
    public Weapons GetWeapon(int id)
    {
        return weaponsArray[id];
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown("u"))
    //     {
    //         SaveGunState();
    //     }
    //     
    //     if (Input.GetKeyDown("i"))
    //     {
    //         RetrieveGunState();
    //     }
    //
    //     if (Input.GetKeyDown("o"))
    //     {
    //         ResetGunState();
    //     }
    // }

 
    void Start()
    {
        TransitionScript.onChangeLevelDelegate += TransitionScript_OnChangeLevelDelegate;
        
        RetrieveGunState();
    }

    
    
    private void SaveGunState()
    {
       // print("saving");
        
        Weapons[] savedWepArr = playerSaveSo.savedWepState;
        ChangeGunState(weaponsArray, savedWepArr);
    }
    
    private void RetrieveGunState()
    {
      //  print("retrieving");
        
        Weapons[] savedWepArr = playerSaveSo.savedWepState;
        ChangeGunState(savedWepArr, weaponsArray);
    }

    
    private void ChangeGunState(Weapons[] source, Weapons[] destination)
    {
        int size = weaponsArray.Length;
        for (int i = 0; i < size; i++)
        {
            Weapons tempDest = destination[i];
            Weapons tempSource = source[i];
            
            tempDest.name = tempSource.name;
            tempDest.WeaponId = tempSource.WeaponId;
            tempDest.TotalAmmo = tempSource.TotalAmmo;
            tempDest.IsUnlocked = tempSource.IsUnlocked;
        }
    }
    
    
    private void TransitionScript_OnChangeLevelDelegate()
    {
        SaveGunState();
    }


    private void ResetGunState()
    {
        Weapons[] reset = playerSaveSo.resetWepState;
        
        ChangeGunState(reset, weaponsArray);
        ChangeGunState(reset, playerSaveSo.savedWepState);
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
