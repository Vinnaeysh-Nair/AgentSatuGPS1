using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAndLines : MonoBehaviour
{
    public Dialogue dialogue;

    /*void Update()
    { 
        if (gameObject.activeSelf)
        {
            //Debug.Log("game object is active");

            if(Input.GetKey("p"))
                TriggerDialogue();
        }
    }*/

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
