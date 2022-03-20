using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAndLines : MonoBehaviour
{
    public Dialogue[] dialogue;
    private int currDialogue = 0;
    private int currSentence;

    //private bool canTalk = true;
    //private float timer = 0.5f;
    private bool firstConversation = true;
    
    void Start()
    {
        //Debug.Log(dialogueCount);
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            if (firstConversation)
            {
                TriggerDialogue();
                firstConversation = false;
                TriggerNextDialogue();
            }

            //canTalk &&
            if (Input.GetKeyDown("e"))
            {
                if (!IsALLDialoguesFinished())
                {
                    if (IsAllSentencesFinished())
                    {
                        currSentence = 0;
                    }
                    TriggerNextDialogue();
                }
                else
                {
                    print("no more");
                }
  
                //canTalk = false;
            }
        }
        //timer -= Time.deltaTime;
        //if (canTalk == false && timer <= 0.0f)
        //{
        //    canTalk = true;
        //    timer = 0.5f;
        //}
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue[currDialogue]);
        currDialogue++;
    }

    public void TriggerNextDialogue()
    {
        FindObjectOfType<DialogueManager>().DisplayNextSentence();
        currDialogue++;
    }

    private bool IsAllSentencesFinished()
    {
        return currSentence > dialogue[currDialogue].sentences.Length - 1;
    }

    private bool IsALLDialoguesFinished()
    {
        return currDialogue > dialogue.Length - 1;
    }

    void OnDisable ()
    {
        firstConversation = true;
        //canTalk = true;
    }
}
