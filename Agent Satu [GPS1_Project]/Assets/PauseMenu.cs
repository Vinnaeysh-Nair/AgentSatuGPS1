using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private float timeScaleBeforePause;
    private bool gameIsPaused = false;
    
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (!gameIsPaused)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }
    }

    private void Pause()
    {
        timeScaleBeforePause = Time.timeScale;
        
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    private void Unpause()
    {
        Time.timeScale = timeScaleBeforePause;
        gameIsPaused = false;
    }
}
