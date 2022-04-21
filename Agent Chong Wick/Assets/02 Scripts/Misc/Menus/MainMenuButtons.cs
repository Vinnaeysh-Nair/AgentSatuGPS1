using UnityEngine;
using Application = UnityEngine.Application;

public class MainMenuButtons : Menu
{
    [SerializeField] private TransitionScript transition;
    [SerializeField] private int sceneToLoad;

    [SerializeField] private GameObject titleMenu;
    [SerializeField] private GameObject optionMenu;

    private int _tutorialSceneIndex = 16;
    
    public void PlayGame()
    {
        PlayUIClick();

        transition.LoadNextLevel(sceneToLoad);
    }

    public void OpenOptions()
    {
        PlayUIClick();
        
        titleMenu.SetActive(false);
        optionMenu.SetActive(true);
    }
    
    public void BackToTitle()
    {
        PlayUIClick();
        
        titleMenu.SetActive(true);
        optionMenu.SetActive(false);
    }

    public void Tutorial()
    {
        PlayUIClick();
        
        transition.LoadNextLevel(_tutorialSceneIndex);
    }
    
    
    public void QuitGame()
    {
        PlayUIClick();
        
        Application.Quit();
    }
}
