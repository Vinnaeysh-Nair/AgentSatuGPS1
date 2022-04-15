using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class PlayerSaveData
{
    public int levelIndex;
    public PlayerInventory.Weapons[] weaponsArray;

    public PlayerSaveData()
    {
        this.levelIndex = 0;
        this.weaponsArray = null;
    }
    public PlayerSaveData(int levelIndex, PlayerInventory.Weapons[] weaponsArray)
    {
        this.levelIndex = levelIndex;
        this.weaponsArray = weaponsArray;
    }
}


public static class ProgressSaving 
{
    private static string _filePath = "/SaveFiles/";
    private static string _fileName = "PlayerSaveData";
    
    public static string dir = $"{Application.dataPath}{_filePath}{_fileName}.json";
 
    private static PlayerSaveData _playerSaveData;
    
    public static void SaveFile()
    {
        string data = JsonConvert.SerializeObject(_playerSaveData, Formatting.Indented);   
        
        File.WriteAllText(dir, data);
    }

    public static bool LoadFile()
    {
        if (!File.Exists(dir)) return false;
        
        string data = File.ReadAllText(dir);
        _playerSaveData = JsonConvert.DeserializeObject<PlayerSaveData>(data);
        
        return true;
    }

    public static void RecordPlayerData(int levelIndex, PlayerInventory.Weapons[] weaponsArray)
    {
        _playerSaveData = new PlayerSaveData(levelIndex, weaponsArray);
    }

    public static void LoadPlayerData(PlayerWeaponSaveSO playerWeaponSaveSo)
    {
        TransitionScript.lastLevelIndex = _playerSaveData.levelIndex;
        playerWeaponSaveSo.savedWepState = _playerSaveData.weaponsArray;
    }
}
