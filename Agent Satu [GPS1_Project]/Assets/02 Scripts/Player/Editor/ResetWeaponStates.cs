using UnityEditor;
using UnityEngine;

public class ResetWeaponStates : EditorWindow
{
    [MenuItem("Window/Custom/ResetWeaponEditor")]
    public static void ShowWindow()
    {
        GetWindow<ResetWeaponStates>();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Reset"))
        {
            FindObjectOfType<PlayerInventory>().ResetGunState();
        }
    }
}
