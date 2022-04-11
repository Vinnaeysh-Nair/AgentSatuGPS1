using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightningEffect : MonoBehaviour
{
    
    [SerializeField] private new Light2D light;
    [SerializeField] private float timer = 0.0f;
    private bool lightningable;
        
    void Start()
    {
        //light = .GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        if (timer == 0)
            timer = 1.0f;
        lightningable = true;
    }

    void Update()
    {
        if (Input.GetKeyDown("k") && lightningable)
        { 
            lightning();
            lightningable = false;
        }

        if(timer<=0)
        {
            lightningable = true;
            timer = 1.0f;
        }
        timer *= Time.deltaTime;
    }

    void lightning()
    {
        light.intensity = 1.0f;
    }

}


