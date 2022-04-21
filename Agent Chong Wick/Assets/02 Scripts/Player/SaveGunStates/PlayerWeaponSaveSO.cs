using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWeaponSaveSO", menuName = "ScriptableObjects/PlayerWeaponSaveSO")]
public class PlayerWeaponSaveSO : ScriptableObject
{
    public PlayerInventory.Weapons[] savedWepState;
    public PlayerInventory.Weapons[] resetWepState;
}
