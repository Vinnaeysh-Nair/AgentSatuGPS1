using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneLoader : MonoBehaviour
{
    public KeyCode reloadSceneKey;

    void Update()
    {
        if (Input.GetKeyDown(reloadSceneKey))
        {
            SceneManager.LoadScene("TestCutscene");
        }
    }
}
