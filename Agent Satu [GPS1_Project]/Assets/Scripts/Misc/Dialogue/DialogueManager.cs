using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
 
    
    public void StartDialogue(Dialogue dialogue)
    {
        nameText.text = dialogue.name;
    }
    
    public void DisplayNextSentence(string sentence)
    {
        dialogueText.text = sentence;
    }

    void EndDialogue()
    {
        //dialogueBox.SetActive(false);
        Debug.Log("End of conversation");
    }
}
