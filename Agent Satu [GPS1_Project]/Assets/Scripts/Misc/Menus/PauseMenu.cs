using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    #region Singleton
    public static PauseMenu Instance;
    void Awake()
    {
        Instance = this;
    }
    #endregion 
    
    private Canvas pauseMenuCanvas;
    
    private float timeScaleBeforePause;
    public bool gameIsPaused = false;


    void Start()
    {
        pauseMenuCanvas = GetComponent<Canvas>();
        pauseMenuCanvas.enabled = false;
    }
    
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
        pauseMenuCanvas.enabled = true;
    }

    public void Unpause()
    {
        Time.timeScale = timeScaleBeforePause;
        
        gameIsPaused = false;
        pauseMenuCanvas.enabled = false;
    }

    public void LoadMainMenu()
    {
        //Unpause to normal timeScale before going to main menu
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void SettingsMenu()
    {
        //pauseMenuCanvas.enabled = false;
        print("not yet implemented");
    }
}
