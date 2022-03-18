using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class InteractPopup : MonoBehaviour
{
    [SerializeField] private Vector2 popLocationOffset;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] popups;

    private Sprite worldInteractPopUp;
    private Sprite speechInteractPopUp;


    private void OnDestroy()
    {
        OpenDoor.onAbleToInteractDelegate -= OpenDoor_OnAbleToInteract;
    }

    void Start()
    {
        spriteRenderer.enabled = false;
        OpenDoor.onAbleToInteractDelegate += OpenDoor_OnAbleToInteract;

        //Swap sprite logics implement later
        worldInteractPopUp = popups[0];
        speechInteractPopUp = popups[1];
    }

    private void OpenDoor_OnAbleToInteract(bool canOpen, Transform buttonTransform)
    {
        SwapSprite(worldInteractPopUp);
        
        if (canOpen)
        {
            Vector2 newPos = (Vector2) buttonTransform.position + popLocationOffset;
            transform.position = newPos;
            
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    private void SwapSprite(Sprite spriteToUse)
    {
        Sprite currSprite = spriteRenderer.sprite;
        if (currSprite != spriteToUse)
        {
            spriteRenderer.sprite = spriteToUse;
        }
    }
}
