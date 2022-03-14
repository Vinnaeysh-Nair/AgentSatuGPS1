using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneLoader : MonoBehaviour
{
    [SerializeField] private TransitionScript transition;
    [SerializeField] private CutsceneSO cutsceneSo;

    
    [SerializeField] private Cutscene[] cutscenesArray;

    [SerializeField] private int currCutscene = 0;
    [SerializeField] private int currPanel = 0;
    [SerializeField] private int currSection = 0;

    
    
    
    [System.Serializable]
    private class Panel
    {
        public Transform panelTransform;
        public Transform[] sections;
    }


    [System.Serializable]
    struct Cutscene
    {
        public Transform cutsceneTransform;
        
        [Header("Info for next level to load")]
        [SerializeField] private string levelToLoadSceneName;
        public int levelToLoadIndex;
        
        
        [Header("Panels")]
        public  Panel[] panels;
    };
 
    void Start()
    {
        foreach (CutsceneSO.CutsceneToLoad cutsceneToLoad in cutsceneSo.cutsceneToLoad)
        {
            if (cutsceneSo.GetLastLevelIndex() == cutsceneToLoad.levelIndexBeforeCutscene)
            {
                print("Last level Index was: " + cutsceneSo.GetLastLevelIndex() + ". Loading: Cutscene " + cutsceneToLoad.cutsceneIndexToLoad);
                currCutscene = cutsceneToLoad.cutsceneIndexToLoad;
            }
        }
       
        
        //Disable all Cutscenes except Cutscene to be played
        for (int i = 0; i < cutscenesArray.Length; i++)
        {
            Cutscene thisCutscene = cutscenesArray[i];
            OpenFirstPanel(thisCutscene);

            if (i != currCutscene)
            {
                thisCutscene.cutsceneTransform.gameObject.SetActive(false);
            }
        }
    }


    void Update()
    {
        if (Input.GetKeyDown("y"))
        {
            LoadCutScene();
        }

        if (Input.GetButtonDown("Interact") && !IsAllCutscenesFinished())
        {
            NextSection();
        }
    }
    
    
    public void LoadCutScene()
    {
        SceneManager.LoadScene("TestCutscene");
    }


    //Disable all Panels except first Panel
    private void OpenFirstPanel(Cutscene thisCutscene)
    {
        for (int j = 0; j < thisCutscene.panels.Length; j++)
        {
            Panel thisPanel = thisCutscene.panels[j];
            
            //Get panel
            GameObject panelObj = thisPanel.panelTransform.gameObject;

            
            //Disable this panel's sections
            Transform[] thisPanelSections = thisPanel.sections;
            foreach (Transform section in thisPanelSections)
            {
                section.gameObject.SetActive(false);
            }
            
            //If not first panel, disable this panel
            if (j > 0)
            {
                panelObj.SetActive(false);
            }
        }
    }
    

    public void NextSection()
    {

        //Display sections and panels if amount doesnt exceed
        if (IsAllSectionsFinished())
        {
            currSection = 0;
            
            if (IsWithinPanelAmount())
            {
                ChangePanel();
            }
            //All panels finished, change to level scene
            else
            {
                if (IsAllCutscenesFinished())
                {
                    //do something after all cutscene finished
                    print("no more cutscene #1");
                }
          
                
                //If all sections loaded, load next panel
                transition.LoadNextLevel(cutscenesArray[currCutscene].levelToLoadIndex);
                
                return;
            }
        }
        
        //Display
        DisplaySection();
        currSection++;
    }

    private bool IsAllCutscenesFinished()
    {
        if (currCutscene >= cutscenesArray.Length - 1)
        {
            return true;
        }

        return false;
    }

    private void DisplaySection()
    {
        GetCurrPanel().sections[currSection].gameObject.SetActive(true);
    }

    private void ChangePanel()
    {
        currPanel++;
        GetCurrPanel().panelTransform.gameObject.SetActive(true);
    }

    private bool IsAllSectionsFinished()
    {
        if (currSection > GetCurrPanel().sections.Length - 1)
        {
            return true;
        }

        return false;
    }

    private bool IsWithinPanelAmount()
    {
        if (currPanel >= cutscenesArray[currCutscene].panels.Length - 1)
        {
            return false;
        }

        return true;
    }

   
    private Panel GetCurrPanel()
    {
        return cutscenesArray[currCutscene].panels[currPanel];
    }
}
