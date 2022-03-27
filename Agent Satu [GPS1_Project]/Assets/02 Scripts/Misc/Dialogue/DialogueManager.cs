using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;


    public TextColor[] textColors;
    
    [System.Serializable]
    public class TextColor
    {
        public int speakerId;
        public Color textColor;
    }
    
    public void StartDialogue(Dialogue dialogue)
    {
        nameText.text = dialogue.name;

        foreach (TextColor txtColor in textColors)
        {
            if (txtColor.speakerId == dialogue.speakerId)
            { 
                nameText.color = txtColor.textColor;
            }
        }
    }
    
    public void DisplayNextSentence(Dialogue dialogue, int sentenceIndex)
    {
        dialogueText.text = dialogue.sentences[sentenceIndex];

        foreach (TextColor txtColor in textColors)
        {
            if (txtColor.speakerId == dialogue.speakerId)
            {
                dialogueText.color = txtColor.textColor;
            }
        }
    }


    void EndDialogue()
    {
        //dialogueBox.SetActive(false);
        Debug.Log("End of conversation");
    }
}
