
using UnityEngine;

public class DialogueLoader : MonoBehaviour
{
    [SerializeField] private TransitionScript transition;
    [SerializeField] private CutsceneSO cutsceneSo;

    [Header("For debugging: ")]
    [SerializeField] private int currDialogue;
    [SerializeField] private int currLine = -1;
    [SerializeField] private int prevLine;
    
    
    [SerializeField] private Dialogue[] dialoguesArray;
    
    [System.Serializable]
    public struct Dialogue
    {
        public int loadId;
        public Transform dialogueTransform;
        
        [Header("Info for next level to load")]
        [SerializeField] private string levelToLoadSceneName;
        public int levelToLoadIndex;

        public Line[] lines;

        //public Conversation conversation;
    }

    [System.Serializable]
    public class Line
    {
        public GameObject line;
        public bool staysInScene = false;
    }

    void Update()
    {
         
        if (Input.GetButtonDown("ProceedCutscene"))
        {
            if(!cutsceneSo.loadCutsceneOrDialogue)
            {
                NextLine();
            }
        }
    }

    void Start()
    {
        for (int i = 0; i < dialoguesArray.Length; i++)
        {
            Dialogue thisDialogue = dialoguesArray[i];
            thisDialogue.dialogueTransform.gameObject.SetActive(false);
        }
        
        // foreach (CutsceneSO.DialogueToLoad dialogueToLoad in cutsceneSo.dialogueToLoad)
        // {
        //     // if (cutsceneSo.GetLastLevelIndex() == dialogueToLoad.levelIndexBeforeDialogue)
        //     // {
        //     //     print("Last level Index was: " + cutsceneSo.GetLastLevelIndex() + ". Loading: Dialogue with index " + dialogueToLoad.dialogueIndexToLoad);     //remove later
        //     //     currDialogue = dialogueToLoad.dialogueIndexToLoad;
        //     // }
        //     
        //
        // }

        // currDialogue = -1;
        // foreach (Dialogue dialogue in dialoguesArray)
        // {
        //     foreach (CutsceneSO.DialogueToLoad dialogueToLoad in cutsceneSo.dialogueToLoad)
        //     {
        //         if (dialogue.loadId == dialogueToLoad.loadId)
        //         {
        //             currDialogue = dialogueToLoad.dialogueIndexToLoad;
        //             print(currDialogue);
        //         }
        //     }
        // }

        if (currDialogue == -1)
        {
            print("no dialogue");
            return;
        }
        OpenFirstLine(dialoguesArray[currDialogue]);
    }
    
    
    private void OpenFirstLine(Dialogue thisDialogue)
    {
        thisDialogue.dialogueTransform.gameObject.SetActive(true);
        thisDialogue.lines[currLine].line.SetActive(true);
    }

    private void NextLine()
    {
        if (!IsAllLinesFinished())
        {
            prevLine = currLine;
            currLine++;
            
            Line previousLine = dialoguesArray[currDialogue].lines[prevLine];
            if (!previousLine.staysInScene)
            {
                previousLine.line.SetActive(false);
            }
            
            Line thisLine = dialoguesArray[currDialogue].lines[currLine];
            thisLine.line.SetActive(true);
        }
        else
        {
            currLine = 0;
            
            transition.LoadNextLevel(dialoguesArray[currDialogue].levelToLoadIndex);
            print("All lines finished");
        }
    }

    private bool IsAllLinesFinished()
    {
        return currLine == dialoguesArray[currDialogue].lines.Length - 1;
    }
    
    
}
