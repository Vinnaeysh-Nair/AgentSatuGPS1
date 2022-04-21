using UnityEngine;

public class SkipableVideo : VideoPlaying
{
    void Update()
    {
        if (Input.GetButtonDown("ProceedInteraction"))
        {
            EndVideo();
        }
    }
}
