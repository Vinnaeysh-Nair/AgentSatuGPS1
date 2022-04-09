using UnityEngine;

public class ZoneToTriggerDialogue : MonoBehaviour
{
    [Header("Ref:")]
    [SerializeField] private bool inTalkingZone = false;
    [SerializeField] private DialogueManager dialogueManager;
    
    private DialogueAndLines dialogueAndLines;
    private GameObject DialogueBox;

    private bool finishedDialogue = false;

    void OnDestroy()
    {
        dialogueAndLines.OnFinishDialogue -= DialogueAndLines_OnFinishDialogue;
    }
    
    void Start()
    {
        dialogueAndLines = dialogueManager.DialogueAndLines;
        DialogueBox = dialogueManager.DialogueBox;
        
        dialogueAndLines.OnFinishDialogue += DialogueAndLines_OnFinishDialogue;
        DialogueBox.SetActive(false);
    }

    void Update()
    {
        if(inTalkingZone)
        {
            TriggeringDialogue();
        }
        else
        {
            DisableDialogue();
        }
    }

    void OnTriggerEnter2D(Collider2D talkingArea)
    {
        if (talkingArea.CompareTag("Player"))
        {
            inTalkingZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D talkingArea)
    {
        if (talkingArea.CompareTag("Player"))
        {
            inTalkingZone = false;
        }
    }

    private void TriggeringDialogue()
    {
        if (finishedDialogue) return;
        
        DialogueBox.SetActive(true);
    }

    private void DisableDialogue()
    {
        DialogueBox.SetActive(false);
    }

    private void DialogueAndLines_OnFinishDialogue()
    {
        finishedDialogue = true;
        dialogueAndLines.OnFinishDialogue -= DialogueAndLines_OnFinishDialogue;
    }
}
