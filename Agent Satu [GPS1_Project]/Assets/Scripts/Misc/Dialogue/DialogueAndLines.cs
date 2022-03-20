using UnityEditor;
using UnityEngine;

public class DialogueAndLines : MonoBehaviour
{
    public Dialogue[] dialogue;

    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private int currDialogue = 0;
    
    [SerializeField] private int currSentence;

    //private bool canTalk = true;
    //private float timer = 0.5f;
    private bool firstConversation = true;
    


    void Update()
    {
        if (gameObject.activeSelf)
        {
            if (Input.GetKeyDown("e"))
            {
                if (!IsAllDialoguesFinished())
                {
                    DisplayNextDialogue();
                    
                    
                    if (IsAllSentencesFinished())
                    {
                        
                        currSentence = 0;
                        currDialogue++;
                        
                        DisplayNextDialogue();
                        TriggerNextSentence();
                    }
                    else
                    {
                        TriggerNextSentence();
                        currSentence++;
                        
                        print("no more sentece");
                    }
                }
                else
                {
                    print("mo more dialogue");
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
        dialogueManager.DisplayNextSentence(dialogue[currDialogue].sentences[currSentence]);
    }

    private bool IsAllSentencesFinished()
    {
        return currSentence >= dialogue[currDialogue].sentences.Length;
    }

    private bool IsAllDialoguesFinished()
    {
        return currDialogue >=  dialogue.Length;
    }

    void OnDisable ()
    {
        firstConversation = true;
        //canTalk = true;
    }
}
