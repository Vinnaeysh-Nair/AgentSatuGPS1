using UnityEngine;

public class ExitPoint : MonoBehaviour
{
    public SceneLoader sceneLoader;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            sceneLoader.LoadNextLevel();
        }
    }
    
}
