using UnityEngine;

public class Menu : MonoBehaviour
{
    
    protected void PlayUIClick()
    {
        SoundManager.Instance.PlayEffect("menuClick");
    }
}
