using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScript : MonoBehaviour
{
    [Header("General")]
    //public KeyCode transitionButton;
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 1f;
    public static int lastLevelIndex;

    [Space] [Header("Cutscenes & dialogue")] 
    [SerializeField] private CutsceneSO cutsceneSo;
    [SerializeField] private bool willTransitionToCutsceneOrDialogue;

    [Header("True for Cutscene; False for Dialogue")]
    [SerializeField] private bool cutsceneOrDialogue;

    
    [Header("Id for cutscene or dialogue to load")]
    [SerializeField] private int loadId;

    
    [SerializeField] private int cutsceneSceneIndex = 8;

    private bool playerEntered = false;

    public delegate void OnChangeLevel();
    public static event OnChangeLevel onChangeLevelDelegate;
    
    // void Update()
    // {
    //     if (Input.GetKeyDown("y"))
    //     {
    //         //LoadNextLevel();
    //         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //     }
    // }

    void Start()
    {
        //If already cutscene dont overwrite
        lastLevelIndex = SceneManager.GetActiveScene().buildIndex;
        if (lastLevelIndex != cutsceneSceneIndex)
        {
            cutsceneSo.loadId = loadId;
            
            if (willTransitionToCutsceneOrDialogue)
            {
                if (cutsceneOrDialogue)
                {
                    cutsceneSo.loadCutsceneOrDialogue = true;
                }
                else
                {
                    cutsceneSo.loadCutsceneOrDialogue = false;
                }
            }
        }
    }

    //Used in levels
    private void LoadNextLevel()
    {
        if (onChangeLevelDelegate != null)
        {
            onChangeLevelDelegate.Invoke();
        }
        
        
        int sceneIndexToLoad = 0;
        if (willTransitionToCutsceneOrDialogue)
        {
            sceneIndexToLoad = cutsceneSceneIndex;
        }
        else
        {
            sceneIndexToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        }
        
        StartCoroutine(LoadLevel(sceneIndexToLoad));
    }


    //Used cutscene
    public void LoadNextLevel(int index)
    {
        StartCoroutine(LoadLevel(index));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadSceneAsync(levelIndex);
    }
    

    void OnTriggerEnter2D(Collider2D entranceCollider)
    {
        if (entranceCollider.CompareTag("Player"))
        {
            if (playerEntered) return;
            playerEntered = true;
            
            LoadNextLevel();
        }
    }
}
