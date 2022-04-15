using UnityEditor;
using UnityEngine;
using System.IO;

public class ResetWeaponStatesEditor : EditorWindow
{
    [MenuItem("Window/Custom/ResetWeaponStatesEditor")]
    public static void ShowWindow()
    {
        GetWindow<ResetWeaponStatesEditor>();
    }

    private void OnGUI()
    {
        
        GUILayout.Label("For PlayerWeaponSaveSo and PlayerInventory's weaponsArray: \nswaps their current states with the default state stored in the SO.");
        
        if (GUILayout.Button("Reset"))
        {
            FindObjectOfType<PlayerInventory>().ResetGunState();
            DeleteSaveFile();
        }
    }
    
    private static void DeleteSaveFile()
    {
        string jsonDir = ProgressSaving.jsonDir;
        if (!File.Exists(jsonDir)) return;
        
        File.Delete(jsonDir);
        
        // string metaDir = $"{ProgressSaving.dir}.meta";
        // File.Delete(metaDir);
        
        AssetDatabase.Refresh();
       // Debug.Log(jsonDir);
       //  AssetDatabase.DeleteAsset(jsonDir);
       //  AssetDatabase.Refresh();
    }
}
