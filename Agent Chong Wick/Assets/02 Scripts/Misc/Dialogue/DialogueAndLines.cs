using UnityEngine;
using System;


public class DialogueAndLines : MonoBehaviour
{
    [Header("Ref:")]
    [SerializeField] private Dialogue[] dialogue;
    [SerializeField] private DialogueManager dialogueManager;
    
    private int currDialogue = 0;
    private int currSentence;

    public event Action OnFinishDialogue;


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
             
                   // print("mo more dialogue");
                    dialogueManager.DialogueBox.SetActive(false);

                    if (OnFinishDialogue != null)
                    {
                        OnFinishDialogue.Invoke();
                    }
                }
                
            }
        }
    }

    private void DisplayNextDialogue()
    {
        dialogueManager.StartDialogue(dialogue[currDialogue]);
    }


    private void TriggerNextSentence()
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
