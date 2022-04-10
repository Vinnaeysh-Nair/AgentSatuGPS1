using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BulletTime : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private GameObject bulletTimeScreenEffect;
    [SerializeField] private BarChangeSlider bulletTimeBar;
    private PauseMenu pauseMenu;
    
    //Fields
    [Header("Settings")]
    [SerializeField] [Range(0f, 1f)] private float slowdownFactor = .02f;
    [SerializeField] [Range(0f, 1f)] private float usage = .05f;
    [SerializeField] [Range(0f, 1f)] private float minGaugeBeforeUse = .3f;
    [SerializeField] private float backToNormalTimeLength = 1f;

    
    [Header("Not for editing")]
    [SerializeField] private float abilityGauge = 1f;
    private bool activated = false;

    private SoundManager _soundManager;

    void Start()
    {
        pauseMenu = PauseMenu.Instance;
        _soundManager = SoundManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseMenu.gameIsPaused) return;
        
        if (abilityGauge <= 0)
        {
            StartCoroutine(DeactivateBulletTime());
        }
        
        if (Input.GetButtonDown("ActivateBulletTime"))
        {
            if (abilityGauge <= 0f) return;

            //Trigger bullet time on
            if (!activated)
            {
                //Minimum gauge before bullet time can be used
                if (abilityGauge <= minGaugeBeforeUse) return;
                ActivateBulletTime();
            }
            //Trigger bullet time off
            else
            {
                StartCoroutine(DeactivateBulletTime());
            }
        }

        //Add or reduce gauge according to activation status
        if (activated)
        {
            DepleteGauge();
            EnableScreenEffect();
        }
        else
        {
            RefillGauge();
            DisableScreenEffect();
        }
        abilityGauge = Mathf.Clamp(abilityGauge, 0f, 1f);
        
        bulletTimeBar.SetBarAmount(abilityGauge);
    }
    
    private void ActivateBulletTime()
    {
        _soundManager.BulletTimePitchDown();
        
        activated = true;
        Time.timeScale *= slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
    
    private IEnumerator DeactivateBulletTime()
    {
        while (Time.timeScale < 1f)
        {
            yield return new WaitForNextFrameUnit();
    
            Time.timeScale += (1f / backToNormalTimeLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            
        }
        activated = false;
        _soundManager.BulletTimePitchReset();
     
    }
    
    private void DepleteGauge()
    {
        if (abilityGauge <= 0f) return;
        abilityGauge -= usage * Time.unscaledDeltaTime;
    }
    
    private void RefillGauge()
    {
        if (abilityGauge > 1f) return;
        abilityGauge += usage * Time.unscaledDeltaTime;
    }

    private void EnableScreenEffect()
    {
        //Automate finding screen effect
        
        if (bulletTimeScreenEffect == null)
        {
            Debug.Log("No image reference to bullet time screen effect");
            return;
        }
        
        Color color = bulletTimeScreenEffect.GetComponent<Image>().color;
        
        if (color.a < 0.25f)
        {
            color.a += 0.01f;
        
            bulletTimeScreenEffect.GetComponent<Image>().color = color;
        }
    }

    private void DisableScreenEffect()
    {
        if (bulletTimeScreenEffect == null) return;
        
        Color color = bulletTimeScreenEffect.GetComponent<Image>().color;
        
        if (color.a > 0f)
        {
            color.a -= 0.01f;
        
            bulletTimeScreenEffect.GetComponent<Image>().color = color;
        }
    }
}
