using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneToTriggerDialogue : MonoBehaviour
{
    [SerializeField] bool inTalkingZone = false;
    public GameObject DialogueBox;
    public Collider2D talkingArea;

    void Start()
    {
        DialogueBox.SetActive(false);
    }

    void Update()
    {
        if(inTalkingZone)
        {
            if (Input.GetKey("p"))
                TriggeringDialogue();
        }
        else
        {
            DisableDialogue();
        }
    }

    void OnTriggerEnter2D(Collider2D talkingArea)
    {
        if (talkingArea.gameObject.tag == "Player")
            inTalkingZone = true;
    }

    void OnTriggerExit2D(Collider2D talkingArea)
    {
        if (talkingArea.gameObject.tag == "Player")
            inTalkingZone = false;
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
