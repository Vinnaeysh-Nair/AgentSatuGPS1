using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSaveSO", menuName = "ScriptableObjects/PlayerSaveSO")]
public class PlayerSaveSO : ScriptableObject
{
    public PlayerInventory.Weapons[] savedWepState;
    public PlayerInventory.Weapons[] resetWepState;
}
