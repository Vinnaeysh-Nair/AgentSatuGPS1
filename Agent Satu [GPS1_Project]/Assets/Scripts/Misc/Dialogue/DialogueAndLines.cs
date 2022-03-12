using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAndLines : MonoBehaviour
{
    public Dialogue dialogue;

    private bool canTalk = true;
    private float timer = 1.0f;
    private int dialogueCount = 0;
    
    void Start()
    {
        //Debug.Log(dialogueCount);
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            if (canTalk && Input.GetKey("e"))
            {
                if (dialogueCount == 0)
                {
                    TriggerDialogue();
                    canTalk = false;
                    dialogueCount++;
                }
                else if (dialogueCount >= 1)
                {
                    TriggerNextDialogue();
                    canTalk = false;
                    dialogueCount++;
                }
            }
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                canTalk = true;
                timer = 1.0f;
            }
            return;
        }
        else if(!(gameObject.activeSelf))
        { 
            dialogueCount = 0; 
        }
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public void TriggerNextDialogue()
    {
        FindObjectOfType<DialogueManager>().DisplayNextSentence();
    }
}
