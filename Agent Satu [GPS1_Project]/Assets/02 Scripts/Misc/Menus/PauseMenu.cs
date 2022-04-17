using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : Menu
{
    #region Singleton
    public static PauseMenu Instance;
    void Awake()
    {
        Instance = this;
    }
    #endregion

    [SerializeField] private GameObject pauseMain;
    [SerializeField] private GameObject options;
    
    
    private Canvas pauseMenuCanvas;
    
    private float timeScaleBeforePause;
    
    [HideInInspector]
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
        CrosshairAiming.ShowCursor(true);
        timeScaleBeforePause = Time.timeScale;
        
        Time.timeScale = 0f;
        gameIsPaused = true;
        pauseMenuCanvas.enabled = true;
    }

    public void Unpause()
    {
        PlayUIClick();
        CrosshairAiming.ShowCursor(false);
        
        Time.timeScale = timeScaleBeforePause;
        
        gameIsPaused = false;
        pauseMenuCanvas.enabled = false;

        if (!pauseMain.activeSelf)
        {
            pauseMain.SetActive(true);
            options.SetActive(false);
        }
    }

    public void LoadMainMenu()
    {
        PlayUIClick();
        
        //Unpause to normal timeScale before going to main menu
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void OpenOptions()
    {
        PlayUIClick();
        
        pauseMain.SetActive(false);
        options.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        PlayUIClick();
        
        pauseMain.SetActive(true);
        options.SetActive(false);
    }

    public void Restart()
    {
        PlayUIClick();
        
        Time.timeScale = 1f;
        gameIsPaused = false;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
