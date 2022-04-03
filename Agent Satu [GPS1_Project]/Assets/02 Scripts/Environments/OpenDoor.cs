using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private Transform buttonInactive;
    [SerializeField] private Transform doorClosed;


    private bool canOpen = false;

    public delegate void OnAbleToInteract(bool canInteract, Transform buttonTransform);
    public static event OnAbleToInteract onAbleToInteractDelegate;
    
    void OnDestroy(){
         PlayerMovement.onInteractDelegate -= PlayerMovement_OnInteract;
    }
    
    void Start()
    {
        PlayerMovement.onInteractDelegate += PlayerMovement_OnInteract;
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            canOpen = true;
            
            if (onAbleToInteractDelegate != null)
            {
                onAbleToInteractDelegate.Invoke(canOpen, transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canOpen = false;
            
            if (onAbleToInteractDelegate != null)
            {
                onAbleToInteractDelegate.Invoke(canOpen, transform);
            }
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
