using UnityEngine;
using TMPro;

public class DisplayAmmoCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clipCountText;
    [SerializeField] private TextMeshProUGUI reserveCountText;

    
    public void SetAmmoCount(int wepId, int clip, int reserve)
    {
        //If pistol, display "infinity symbol"
        if (wepId == 0)
        {
            clipCountText.text = "\u221E";
            reserveCountText.text = "/ " + "\u221E";
        }
        else
        {
            clipCountText.text = clip.ToString();
            reserveCountText.text = "/ " + reserve;
        }
    }
}
