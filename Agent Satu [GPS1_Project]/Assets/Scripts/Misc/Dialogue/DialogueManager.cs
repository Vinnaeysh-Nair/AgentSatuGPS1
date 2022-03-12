using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    private Queue<string> dialogueLine;

    void Start()
    {
        dialogueLine = new Queue<string>();
        //nameText = GameObject.Find("Name").GetComponent<Text>;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        nameText.text = dialogue.name;
        //Debug.Log("Talking to " + dialogue.name);

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

        string tempSentence = dialogueLine.Dequeue();
        //Debug.Log(tempSentence);
        
        dialogueText.text = tempSentence;
        //dialogueText.text = dialogueLine.Dequeue();
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation");
    }
}
