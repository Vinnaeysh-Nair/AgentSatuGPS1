using UnityEngine;

public class DialogueAndLines : MonoBehaviour
{
    public Dialogue[] dialogue;

    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private int currDialogue = 0;
    
    [SerializeField] private int currSentence;

  //  public delegate void OnFinishDialogue();

   // public static event OnFinishDialogue onFinishDialogueDelegate;

    //private int prevDialogue;     //loop back to first dialogue


    void Start()
    {
        DisplayNextDialogue();
        TriggerNextSentence();

        currSentence++;
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            if (Input.GetButtonDown("Interact"))
            {
                if (!IsAllDialoguesFinished())
                {
                    DisplayNextDialogue();
                    
                    if (IsAllSentencesFinished())
                    {
                       
                        currSentence = 0;
                      

                      //  prevDialogue = currDialogue;
                        currDialogue++;
                        
                        DisplayNextDialogue();
                        TriggerNextSentence();
                    }
                    else
                    {
                        TriggerNextSentence();
                        currSentence++;
                       // print("no more sentece");
                    }
                }
                else
                {
                   // currDialogue = prevDialogue;
                   // print("mo more dialogue");

                    // if (onFinishDialogueDelegate != null)
                    // {
                    //     onFinishDialogueDelegate.Invoke();       //trigger boss start after dialogue, not implementing yet
                    // }
                }
                
            }
        }
    }

    public void DisplayNextDialogue()
    {
        dialogueManager.StartDialogue(dialogue[currDialogue]);
    }


    public void TriggerNextSentence()
    {
        dialogueManager.DisplayNextSentence(dialogue[currDialogue], currSentence);
    }

    private bool IsAllSentencesFinished()
    {
        if (currSentence < dialogue[currDialogue].sentences.Length-1)
        {
            return false;
        }
        return true;
    }

    private bool IsAllDialoguesFinished()
    {
        if (currDialogue < dialogue.Length - 1)
        {
            return false;
        }
        
        return true;
    }
    
}
