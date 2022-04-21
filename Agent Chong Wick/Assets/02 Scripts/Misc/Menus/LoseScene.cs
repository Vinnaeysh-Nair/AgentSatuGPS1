using UnityEngine;

public class LoseScene : Menu
{
    [SerializeField] private TransitionScript _transitionScript;
    
    public void Restart()
    {
        PlayUIClick();
        _transitionScript.Restart();
    }

    public void ReturnToMainMenu()
    {
        PlayUIClick();
        _transitionScript.ReturnToMainMenu();
    }
}
