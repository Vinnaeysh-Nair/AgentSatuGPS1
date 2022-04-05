using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Experimental.Rendering.Universal;

public class LightController : MonoBehaviour
{
    //Light2D like;
    float thunderTiming;
    float timer;

    void Start()
    {
        like = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        //thunderTiming = 10.0f;
        timer = 5.0f;
    }

    void Update()
    {
        if(timer <= 0)
        {

        }
    }

    void FixedUpdate()
    {
        timer = timer * Time.deltaTime;
    }
}
