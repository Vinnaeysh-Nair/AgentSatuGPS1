using UnityEngine;


public class LevelSelect : MonoBehaviour
{
    [SerializeField] private TransitionScript transitionScript;
    [SerializeField] private int indexToLoad;
    

    public void ProceedToLevel()
    {
      transitionScript.LoadNextLevel(indexToLoad);
    }
}
