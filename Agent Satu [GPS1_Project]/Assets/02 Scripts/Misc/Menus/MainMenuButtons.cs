using UnityEngine;
using UnityEngine.SceneManagement;
using Application = UnityEngine.Application;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private TransitionScript transition;
    [SerializeField] private int sceneToLoad;

    [SerializeField] private GameObject titleMenu;
    [SerializeField] private GameObject optionMenu;
    public void PlayGame()
    {
        //SceneManager.LoadSceneAsync(sceneToLoad);
        transition.LoadNextLevel(sceneToLoad);
    }

    public void OpenOptions()
    {
        titleMenu.SetActive(false);
        optionMenu.SetActive(true);
    }
    
    public void BackToTitle()
    {
        titleMenu.SetActive(true);
        optionMenu.SetActive(false);
    }
    
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
