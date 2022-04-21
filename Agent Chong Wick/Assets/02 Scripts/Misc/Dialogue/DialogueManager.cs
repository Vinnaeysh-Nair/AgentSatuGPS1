using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Ref:")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private DialogueAndLines dialogueAndLines;
    
    [Header("Settings:")]
    [SerializeField] private TextColor[] textColors;

    
    [System.Serializable]
    public class TextColor
    {
        public int speakerId;
        public Color textColor;
    }

    public GameObject DialogueBox
    {
        get => dialogueBox;
    }

    public DialogueAndLines DialogueAndLines
    {
        get => dialogueAndLines;
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
}
