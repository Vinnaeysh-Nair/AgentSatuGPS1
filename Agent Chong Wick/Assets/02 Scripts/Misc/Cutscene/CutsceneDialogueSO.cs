using UnityEngine;

[CreateAssetMenu(fileName = "CutsceneDialogueSO", menuName = "ScriptableObjects/CutsceneDialogueSO")]
public class CutsceneDialogueSO : ScriptableObject
{
    public int loadId;

    [Header("True for Cutscene; False for Dialogue")]
    public bool loadCutsceneOrDialogue;
}
