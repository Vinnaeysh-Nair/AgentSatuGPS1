using UnityEngine;

public  class PlayerMain : MonoBehaviour
{
    [SerializeField] private PlayerWeaponSaveSO playerWeaponSaveSo;
    private PlayerHpSystem playerHpSystem;
    
    #region Singleton
    public static PlayerMain Instance;
    void Awake()
    {
        Instance = this;
    }
    #endregion

    public PlayerHpSystem PlayerHpSystem
    {
        get => playerHpSystem;
    }
    
    private void OnDestroy()
    {
        TransitionScript.OnChangeLevel -= SavePlayerProgress;
    }
    

    void Start()
    {
        TransitionScript.OnChangeLevel += SavePlayerProgress;

        playerHpSystem = GetComponent<PlayerHpSystem>();

        LoadPlayerProgress();
    }
    
    private void SavePlayerProgress()
    {
        ProgressSaving.RecordPlayerData(TransitionScript.lastLevelIndex, playerWeaponSaveSo.savedWepState);
        ProgressSaving.SaveFile();
    }

    private  void LoadPlayerProgress()
    {
        if (ProgressSaving.LoadFile())
        {
            ProgressSaving.LoadPlayerData(playerWeaponSaveSo);
        }
    }
} 
