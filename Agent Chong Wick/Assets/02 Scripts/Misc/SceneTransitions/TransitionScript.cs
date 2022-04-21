using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class TransitionScript : MonoBehaviour
{
    //scene index info
    private static int mainMenuIndex = 1;
    private static int cutsceneSceneIndex = 15;
    private static int tutorialSceneIndex = 16;
    
    
    //If is Lose Scene, dont override the lastLevelIndex
    [Header("If current scene is Lose Scene")]
    [SerializeField] private bool isLoseScene = false;
    
    public static int lastLevelIndex = 0;
    public static int currLevelIndex = 0;
            
    [Header("General")]
    [SerializeField] private float transitionTime = 1f;
    private Animator transition;
    
    [Space] [Header("Cutscenes & dialogue")] 
    [SerializeField] private CutsceneDialogueSO cutsceneDialogueSo;
    [SerializeField] private bool willTransitionToCutsceneOrDialogue;

    [Header("True for Cutscene; False for Dialogue")]
    [SerializeField] private bool cutsceneOrDialogue;

    
    [Header("Id for cutscene or dialogue to load")]
    [SerializeField] private int loadId;
    
    private bool playerEntered = false;


    public static event Action OnChangeLevel;
    public static event Action<int> OnSceneChange;


    void Awake()
    {
        transition = transform.GetChild(0).GetComponent<Animator>();
        transition.enabled = false;
    }
    
    void Start()
    {
        currLevelIndex = SceneManager.GetActiveScene().buildIndex;
        if(OnSceneChange != null) OnSceneChange.Invoke(currLevelIndex);
        
        if (isLoseScene) return;
            
        //If already cutscene dont overwrite
        lastLevelIndex = currLevelIndex;
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

            if (!IsTutorialScene())
                LoadNextLevel();
            else
                ReturnToMainMenu();
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

    private IEnumerator LoadLevel(int levelIndex)
    {
        transition.enabled = true;
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

    public static bool IsTutorialScene()
    {
        if (currLevelIndex == tutorialSceneIndex) return true;
        return false;
    }
}
