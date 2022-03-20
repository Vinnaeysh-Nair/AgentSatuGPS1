using UnityEngine;

[CreateAssetMenu(fileName = "CutsceneSO", menuName = "ScriptableObjects/CutsceneSO")]
public class CutsceneSO : ScriptableObject
{
    public int loadId;

    [Header("True for Cutscene; False for Dialogue")]
    public bool loadCutsceneOrDialogue;
    
}
