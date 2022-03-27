using UnityEngine;
using UnityEngine.SceneManagement;

public class PrevLevelTracker : MonoBehaviour
{
    private int currLevelIndex;
    [SerializeField] private CutsceneSO cutsceneSo;

    
    private void Start()
    {
        currLevelIndex = SceneManager.GetActiveScene().buildIndex;
       // cutsceneSo.SetLastLevelIndex(currLevelIndex);
    }
}
