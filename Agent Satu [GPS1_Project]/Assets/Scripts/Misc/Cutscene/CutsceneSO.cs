using UnityEngine;

[CreateAssetMenu(fileName = "CutsceneSO", menuName = "ScriptableObjects/CutsceneSO")]
public class CutsceneSO : ScriptableObject
{
    private int lastLevelIndex;

    [Header("Load cutscene based on level")]
    public CutsceneToLoad[] cutsceneToLoad;

    [System.Serializable]
    public class CutsceneToLoad
    {
        public int levelIndexBeforeCutscene = 0;
        public int cutsceneIndexToLoad;
    };

    public void SetLastLevelIndex(int lastLevelIndex)
    {
        this.lastLevelIndex = lastLevelIndex;
    }

    public int GetLastLevelIndex()
    {
        return lastLevelIndex;
    }
}
