using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScript : MonoBehaviour
{
    [Header("General")]
    //public KeyCode transitionButton;
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 1f;

    [Space]
    [Header("Cutscenes")]
    [SerializeField] private bool willTransitionToCutscene;
    [SerializeField] private int cutsceneSceneIndex = 8;


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

    

    //Used in levels
    private void LoadNextLevel()
    {
        if (onChangeLevelDelegate != null)
        {
            onChangeLevelDelegate.Invoke();
        }
        
        
        int sceneIndexToLoad = 0;
        if (willTransitionToCutscene)
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
            LoadNextLevel();
        }
    }
}
