using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //Components
    public Animator anim;
    
    //Fields
    public float transitionTime = 1f;

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

 
    private IEnumerator LoadLevel(int levelIndex)
    {
        anim.SetTrigger("Start");

        yield return new WaitForSeconds(.5f);

        SceneManager.LoadScene(levelIndex);
    }
    
    public void LoadLoseScene()
    {
        StartCoroutine(LoadScene());
    }
    
    private IEnumerator LoadScene()
    {
        anim.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("Lose Scene");
    }


}
