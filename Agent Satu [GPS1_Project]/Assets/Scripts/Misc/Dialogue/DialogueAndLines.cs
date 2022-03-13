using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAndLines : MonoBehaviour
{
    public Dialogue dialogue;

    private bool canTalk = true;
    private float timer = 0.5f;
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
            }
            
            if (canTalk && Input.GetKey("e"))
            {
                TriggerNextDialogue();
                canTalk = false;
            }
        }
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            canTalk = true;
            timer = 0.5f;
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

    void OnDisable ()
    {
        firstConversation = true;
    }
}
