using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAndLines : MonoBehaviour
{
    public Dialogue dialogue;

    private bool canTalk = true;
    private float timer = 1.0f;
    //private int dialogueCount = 0;
    void Update()
    {
        if (gameObject.activeSelf)
        {
            if (canTalk && Input.GetKey("e"))
            {
                TriggerDialogue();
                canTalk = false;
            }

            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                timer = 1.0f;
                canTalk = true;
                //dialogue++;
            }
        }

        //if (!(gameObject.activeSelf))
        //{
        //    dialogue = 0;
        //}
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    //public void TriggerNextDialogue()
    //{
    //    FindObjectOfType<DialogueManager>().DisplayNextSentence(Dialogue);
    //}
}
