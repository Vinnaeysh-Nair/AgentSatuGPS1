using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneToTriggerDialogue : MonoBehaviour
{
    [SerializeField] bool inTalkingZone = false;
    public GameObject DialogueBox;
    public Collider2D talkingArea;
    public DialogueLocation dialogueLocation;
    private Vector3 tempPos;

    void Start()
    {
        DialogueBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        tempPos = dialogueLocation.DialoguePosition();
        //Debug.Log(tempPos);
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
        //Instantiate(DialogueBox, tempPos, Quaternion.identity);
        //, new Vector3(), Quaternion.identity
    }

    void DisableDialogue()
    {
        DialogueBox.SetActive(false);
    }
}
