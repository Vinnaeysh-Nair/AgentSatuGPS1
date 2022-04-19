using UnityEngine;

public class MiyaAtk4FlashStart : MonoBehaviour
{
    [SerializeField] private MiyaPatterns miyaPatterns;


    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            miyaPatterns.BlindPlayer();
        }
    }

    //called in Animator
    public void DisableFlashStart()
    {
        gameObject.SetActive(false);
    }
}
