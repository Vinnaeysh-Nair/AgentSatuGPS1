using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayAmmoCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clipCountText;
    [SerializeField] private TextMeshProUGUI reserveCountText;

    
    [SerializeField] private Image[] wepNameAndArt;

    public static DisplayAmmoCount Instance;
    private PlayerWeapon _playerWeapon;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        _playerWeapon.onAmmoUpdateDelegate -= PlayerWeapon_OnAmmoUpdate;
    }

    private void Start()
    {
        _playerWeapon = GetComponent<PlayerWeapon>();
        _playerWeapon.onAmmoUpdateDelegate += PlayerWeapon_OnAmmoUpdate;
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

    private void PlayerWeapon_OnAmmoUpdate()
    {
        UpdateAmmoDisplay(_playerWeapon.WepId, _playerWeapon.CurrClip, _playerWeapon.CurrReserve);
    }
}
