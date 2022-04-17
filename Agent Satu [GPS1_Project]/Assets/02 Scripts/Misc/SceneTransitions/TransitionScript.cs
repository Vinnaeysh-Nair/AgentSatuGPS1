using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class TransitionScript : MonoBehaviour
{
    //If is Lose Scene, dont override the lastLevelIndex
    [Header("If current scene is Lose Scene")]
    [SerializeField] private bool isLoseScene = false;
    public static int lastLevelIndex = 0;
            
    [Header("General")]
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 1f;
    
    [Space] [Header("Cutscenes & dialogue")] 
    [SerializeField] private CutsceneDialogueSO cutsceneDialogueSo;
    [SerializeField] private bool willTransitionToCutsceneOrDialogue;

    [Header("True for Cutscene; False for Dialogue")]
    [SerializeField] private bool cutsceneOrDialogue;

    
    [Header("Id for cutscene or dialogue to load")]
    [SerializeField] private int loadId;

    private int mainMenuIndex = 1;
    private int cutsceneSceneIndex = 15;

    private bool playerEntered = false;


    public static event Action OnChangeLevel;
    public static event Action<int> OnSceneChange;
    

    void Start()
    {
        int currIndex = SceneManager.GetActiveScene().buildIndex;
        if(OnSceneChange != null) OnSceneChange.Invoke(currIndex);
        
        if (isLoseScene) return;
            
        //If already cutscene dont overwrite
        lastLevelIndex = currIndex;
        if (lastLevelIndex != cutsceneSceneIndex)
        {
            cutsceneDialogueSo.loadId = loadId;
            
            if (willTransitionToCutsceneOrDialogue)
            {
                if (cutsceneOrDialogue)
                {
                    cutsceneDialogueSo.loadCutsceneOrDialogue = true;
                }
                else
                {
                    cutsceneDialogueSo.loadCutsceneOrDialogue = false;
                }
            }
        }
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

    //Used in levels
    private void LoadNextLevel()
    {
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
        if(OnChangeLevel != null) OnChangeLevel.Invoke();
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
        if(OnSceneChange != null) OnSceneChange.Invoke(levelIndex);
    }
    


    
    //Use in Lose Scene
    public void Restart()
    {
        StartCoroutine(LoadLevel(PlayerHpSystem.deathLevelIndex));
    }

    //Used in Win/Lose Scenes
    public void ReturnToMainMenu()
    {
        StartCoroutine(LoadLevel(mainMenuIndex));
    }
}
