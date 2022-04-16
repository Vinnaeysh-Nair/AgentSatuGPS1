using UnityEngine;

public class MiyaAtk4FlashStart : MonoBehaviour
{
    private MiyaPatterns _miyaPatterns;

    void Start()
    {
        _miyaPatterns = transform.parent.GetComponent<MiyaPatterns>();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            _miyaPatterns.BlindPlayer();
        }
    }

    //called in Animator
    public void DisableFlashStart()
    {
        gameObject.SetActive(false);
    }
}
