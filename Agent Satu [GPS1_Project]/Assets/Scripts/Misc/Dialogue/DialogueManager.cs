using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> dialogueLine;


    void Start()
    {
        dialogueLine = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting conversation with " + dialogue.name);

        dialogueLine.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            dialogueLine.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (dialogueLine.Count == 0)
        {
            EndDialogue();
            return;
        }

        //string tempSentence = dialogueLine.Dequeue();
        //Debug.Log(tempSentence);
        Debug.Log(dialogueLine.Dequeue());
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation");
    }
}
