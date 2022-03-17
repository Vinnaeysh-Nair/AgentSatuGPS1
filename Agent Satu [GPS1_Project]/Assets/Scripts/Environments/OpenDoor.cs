using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private Transform buttonInactive;
    [SerializeField] private Transform doorClosed;


    private bool canOpen = false;

    void Start()
    {
        PlayerMovement.onInteractDelegate += PlayerMovement_OnInteract;
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canOpen = false;
        }
    }

    private void PlayerMovement_OnInteract()
    {
        if (canOpen)
        {
            ChangeButtonVisual();
            ActivateDoor();
            PlayerMovement.onInteractDelegate  -= PlayerMovement_OnInteract;
        }
    }

    private void ActivateDoor()
    {
        doorClosed.gameObject.SetActive(false);
    }

    private void ChangeButtonVisual()
    {
        buttonInactive.gameObject.SetActive(false);
    }
}
