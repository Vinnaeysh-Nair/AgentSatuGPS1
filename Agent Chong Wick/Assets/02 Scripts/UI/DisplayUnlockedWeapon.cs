using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class DisplayUnlockedWeapon : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI  newWeaponText;
    [SerializeField] [Range(0f, 5f)] private float timeBeforeDisabled = 3f;
    [SerializeField] private Image[] weaponImages;

    private Image _lastImage;
 
    #region Singleton
    public static DisplayUnlockedWeapon Instance;
    void Awake()
    {
        Instance = this;
    }
    #endregion
    
    
    public void DisplayWeapon(int id)
    {
        newWeaponText.enabled = true;
        
        int index = id - 1;
        for (int i = 0; i < weaponImages.Length; i++)
        {
            if (i == index)
            {
                Image foundImage = weaponImages[index];
                foundImage.enabled = true;

                if (_lastImage != null)
                    _lastImage.enabled = false;

                StartCoroutine(StopDisplaying(foundImage));

                _lastImage = foundImage;
            }
        }
    }

    private IEnumerator StopDisplaying(Image wepImage)
    {
        yield return new WaitForSeconds(timeBeforeDisabled);
        
        newWeaponText.enabled = false;
        wepImage.enabled = false;
    }
}
