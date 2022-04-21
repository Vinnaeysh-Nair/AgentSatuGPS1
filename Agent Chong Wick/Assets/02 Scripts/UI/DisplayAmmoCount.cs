using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayAmmoCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clipCountText;
    [SerializeField] private TextMeshProUGUI reserveCountText;

    
    [SerializeField] private Image[] wepNameAndArt;

    private PlayerWeapon _playerWeapon;


    void OnDestroy()
    {
        PlayerWeapon.OnAmmoUpdate -= PlayerWeapon_OnAmmoUpdate;
    }
    
    
    void Start()
    {
        PlayerWeapon.OnAmmoUpdate  += PlayerWeapon_OnAmmoUpdate;
    }


    private void SetAmmoCount(int wepId, int clip, int reserve)
    {
        //If pistol, display "infinity symbol"
        if (wepId == 0)
        {
            clipCountText.text = "\u221E";
            reserveCountText.text = "/ " + "\u221E";
        }
        else
        {
            clipCountText.text = $"{clip}";
            reserveCountText.text = $"/ {reserve}";
        }
    }

    private void SetWepArt(int wepId)
    {
        foreach (Image art in wepNameAndArt)
        {
            art.enabled = false;
        }

        wepNameAndArt[wepId].enabled = true;
    }

    private void UpdateAmmoDisplay(int wepId, int clip, int reserve)
    {
        SetAmmoCount(wepId, clip, reserve);
        SetWepArt(wepId);
    }

    private void PlayerWeapon_OnAmmoUpdate(int id, int clip, int reserve)
    {
        UpdateAmmoDisplay(id, clip, reserve);
    }
}
