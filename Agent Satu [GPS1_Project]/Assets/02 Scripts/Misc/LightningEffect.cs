using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightningEffect : MonoBehaviour
{
    //private UnityEngine.Rendering.Universal.Light2D light;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D light;
    [SerializeField] private float lightTimer = 0.0f;
    [Range(0.1f,0.5f)]
    [SerializeField] private float fadeIntensity = 0.1f;
    private bool lightningable = true;
    private float tempTimer;
        
    void Start()
    {
        //light = .GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        if (lightTimer == 0)
            lightTimer = 1.0f;
    }

    void Update()
    {
        if (Input.GetKeyDown("k") && lightningable)
        {
            Debug.Log("Lag");
            lightning();
            lightningable = false;
        }

        if (!lightningable)
        {
            tempTimer -= Time.deltaTime;
        }

        if (tempTimer <= 0)
        {
            lightningable = true;
            tempTimer = lightTimer;
            StartCoroutine(fadeBackToDark());
        }
    }

    void lightning()
    {
        light.intensity = 1.0f;
    }

    IEnumerator fadeBackToDark()
    { 
        for(float timer2 = 1.0f; timer2 >= 0.0f; timer2 -= 0.1f)
        {
            //must be within 1 and 0.1
            light.intensity -= fadeIntensity;
            yield return new WaitForSeconds(0.05f);
        }
    }
}


