using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private Transform buttonInactive;
    [SerializeField] private Transform doorClosed;

    private PlayerMovement playerMovement;
    private bool canOpen = false;

    void Start()
    {
        playerMovement = transform.Find("/Player/PlayerBody").GetComponent<PlayerMovement>();
        playerMovement.OnInteract += PlayerMovement_OnInteract;
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

    private void PlayerMovement_OnInteract(object sender, System.EventArgs e)
    {
        if (canOpen)
        {
            ChangeButtonVisual();
            ActivateDoor();
            playerMovement.OnInteract -= PlayerMovement_OnInteract;
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
