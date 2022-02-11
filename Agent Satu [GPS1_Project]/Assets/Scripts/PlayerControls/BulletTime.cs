using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BulletTime : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private GameObject bulletTimeScreenEffect;
    
    //Fields
    [Header("Settings")]
    [SerializeField] [Range(0f, 1f)] private float slowdownFactor = .02f;
    [SerializeField] [Range(0f, 1f)] private float usage = .05f;
    [SerializeField] [Range(0f, 1f)] private float minGaugeBeforeUse = .3f;
    [SerializeField] private float backToNormalTimeLength = 1f;

    
    [Header("Not for editing")]
    [SerializeField] private float abilityGauge = 1f;
    private bool activated = false;

    // Update is called once per frame
    void Update()
    {
        abilityGauge = Mathf.Clamp(abilityGauge, 0f, 1f);

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
                //EnableScreenEffect();
            }
            //Trigger bullet time off
            else
            {
                StartCoroutine(DeactivateBulletTime());
                //DisableScreenEffect();
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
    }
    
    private void ActivateBulletTime()
    {
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
    }
    
    private void DepleteGauge()
    {
        if (abilityGauge <= 0) return;
        abilityGauge -= usage * Time.unscaledDeltaTime;
    }
    
    private void RefillGauge()
    {
        if (abilityGauge > 1) return;
        abilityGauge += usage * Time.unscaledDeltaTime;
    }

    private void EnableScreenEffect()
    {
        // Color color = bulletTimeScreenEffect.GetComponent<Image>().color;
        // color.a = .3f;
        //
        // bulletTimeScreenEffect.GetComponent<Image>().color = color;
        
        if (bulletTimeScreenEffect != null)
        {
            Color color = bulletTimeScreenEffect.GetComponent<Image>().color;
        
            if (color.a < 0.25f)
            {
                color.a += 0.01f;
        
                bulletTimeScreenEffect.GetComponent<Image>().color = color;
            }
        }
    }

    private void DisableScreenEffect()
    {
        if (bulletTimeScreenEffect != null)
        {
            Color color = bulletTimeScreenEffect.GetComponent<Image>().color;
        
            if (color.a > 0f)
            {
                color.a -= 0.01f;
        
                bulletTimeScreenEffect.GetComponent<Image>().color = color;
            }
        }

    }
}
