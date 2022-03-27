using UnityEngine;

public class ZoneToTriggerDialogue : MonoBehaviour
{
    [SerializeField] bool inTalkingZone = false;
    public GameObject DialogueBox;
    
    
    void Start()
    {
        DialogueBox.SetActive(false);
    }

    void Update()
    {
        if(inTalkingZone)
        {
            //if (Input.GetKey("p"))//use this for dialogue trigger by button
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

    void TriggeringDialogue()
    {
        DialogueBox.SetActive(true);
    }

    void DisableDialogue()
    {
        DialogueBox.SetActive(false);
    }
}
