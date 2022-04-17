using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightningEffect : MonoBehaviour
{
    
    [SerializeField] private new Light2D light;
    [SerializeField] private float timer = 0.0f;
    private bool lightningable = true;
    private float[] lightningTimer;

    void Start()
    {
        //light = .GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        if (timer == 0)
            timer = 1.0f;
        lightningable = true;

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
            StartCoroutine(returnBackToNormal());
        }
        
    }

    void lightning()
    {
        light.intensity = 1.0f;
    }

    IEnumerator returnBackToNormal()
    {
        lightningable = true;

        yield return new WaitForSeconds(timer);
    }

    IEnumerator generateLightning()
    {
        while (true)
        {
            int temp = Random.Range(0, 3);
            //Debug.Log(lightningTimer[temp]);

            if (lightningable)
            {
                lightning();
                lightningable = false;
            }
            //Debug.Log("lightning triggered");
            yield return new WaitForSeconds(lightningTimer[temp]);  
        }
    }
}


