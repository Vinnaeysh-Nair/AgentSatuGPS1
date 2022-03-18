using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class DisplayUnlockedWeapon : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI  newWeaponText;
    [SerializeField] [Range(0f, 5f)] private float timeBeforeDisabled = 3f;
    [SerializeField] private WeaponImage[] weaponImages;
  


    public static DisplayUnlockedWeapon Instance;
    #region Singleton
    void Awake()
    {
        Instance = this;
    }
    #endregion

    [System.Serializable]

    public class WeaponImage
    {
        public Image image;
        public int id;
    }

   
    public void DisplayWeapon(int id)
    {
        newWeaponText.enabled = true;
        
        int index = id - 1;
        foreach (WeaponImage wepImage in weaponImages)
        {
            if (wepImage.id == index)
            {
              
                
                WeaponImage foundImage = weaponImages[index];
                foundImage.image.enabled = true;

                StartCoroutine(StopDisplaying(foundImage));
            }
        }
    }

    private IEnumerator StopDisplaying(WeaponImage wepImage)
    {
        yield return new WaitForSeconds(timeBeforeDisabled);
        
        newWeaponText.enabled = false;
        wepImage.image.enabled = false;
    }
}
