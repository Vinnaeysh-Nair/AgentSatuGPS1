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
    private int cutsceneSceneIndex = 5;
    
    // void Update()
    // {
    //     if (Input.GetKeyDown(transitionButton))
    //     {
    //         LoadNextLevel();
    //     }
    // }


    private void LoadNextLevel()
    {
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


    public void LoadNextLevel(int index)
    {
        StartCoroutine(LoadLevel(index));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
    

    void OnTriggerEnter2D(Collider2D entranceCollider)
    {
        if (entranceCollider.CompareTag("Player"))
        {
            LoadNextLevel();
        }
    }
}
