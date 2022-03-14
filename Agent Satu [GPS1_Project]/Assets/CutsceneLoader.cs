using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneLoader : MonoBehaviour
{
    public KeyCode reloadSceneKey;

    [SerializeField] private Panel[] panels;

    private int currPanel = 0;
    private int currSection = 0;
    
    [System.Serializable]
    private class Panel
    {
        public Transform panelTransform;
        public Transform[] sections;
    }

    void Start()
    {
        foreach (Panel panel in panels)
        {
            panel.panelTransform.gameObject.SetActive(false);
        }

        for (int i = 0; i < panels.Length; i++)
        {
            GameObject panelObj = panels[i].panelTransform.gameObject;
            
            if (i > 0)
            {
                panelObj.SetActive(false);
            }
        }
    }
    
    
    public void LoadCutScene()
    {
        SceneManager.LoadScene("TestCutscene");
    }

    public void NextSection()
    {
        panels[currPanel]
    }
}
