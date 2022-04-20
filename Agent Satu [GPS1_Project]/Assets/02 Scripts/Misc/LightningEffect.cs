using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightningEffect : MonoBehaviour
{
    [SerializeField] private new Light2D light;
    [SerializeField] private float lightTimer = 0.0f;
    [Range(0.1f, 0.5f)]
    [SerializeField] private float fadeIntensity = 0.1f;
    private bool lightningable = true;
    private float tempTimer;
    private float[] lightningTimer;
    private SoundManager sManager;

    void Start()
    {
        sManager = SoundManager.Instance;
        

        if (lightTimer == 0)
            lightTimer = 1.0f;
        tempTimer = lightTimer;

        lightningTimer = new float[3];
        lightningTimer[0] = 6.0f;
        lightningTimer[1] = 8.0f;
        lightningTimer[2] = 10.0f;

        StartCoroutine(generateLightning());
    }

    void Update()
    {
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

        int temp = Random.Range(1, 4);
        if(temp == 1) 
            sManager.PlayEffect("Thunder1");
        else if(temp == 2) 
            sManager.PlayEffect("Thunder2");
        else 
            sManager.PlayEffect("Thunder3");
    }

    IEnumerator fadeBackToDark()
    {
        for (float timer2 = 1.0f; timer2 >= 0.0f; timer2 -= 0.1f)
        {
            //must be within 1 and 0.1
            if(!(light.intensity <= 0.2))
                light.intensity -= fadeIntensity;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator generateLightning()
    {
        while (true)
        {
            int temp = Random.Range(0, 3);

            if (lightningable)
            {
                lightning();
                lightningable = false;
            }
            yield return new WaitForSeconds(lightningTimer[temp]);
        }
    }
}