using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneLoader : MonoBehaviour
{
    public KeyCode reloadSceneKey;

    [SerializeField] private Panel[] panels;
    
    public class Panel
    {
        public Transform panel;
        public Transform[] sections;
    }

    void Update()
    {
        if (Input.GetKeyDown(reloadSceneKey))
        {
            SceneManager.LoadScene("TestCutscene");
        }
    }
}
