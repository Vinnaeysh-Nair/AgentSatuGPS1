using UnityEngine;
using UnityEngine.SceneManagement;

public class PrevLevelTracker : MonoBehaviour
{
    private int currLevelIndex;
    [SerializeField] private CutsceneDialogueSO cutsceneDialogueSo;

    
    private void Start()
    {
        currLevelIndex = SceneManager.GetActiveScene().buildIndex;
    }
}
