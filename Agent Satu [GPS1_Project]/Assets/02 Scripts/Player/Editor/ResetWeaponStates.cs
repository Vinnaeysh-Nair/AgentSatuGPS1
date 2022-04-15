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
        string dir = ProgressSaving.dir;
        if (!File.Exists(dir)) return;
        
        File.Delete(dir);
        AssetDatabase.Refresh();
    }
}
