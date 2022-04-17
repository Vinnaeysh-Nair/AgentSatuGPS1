using UnityEngine;


public  class PlayerMain : MonoBehaviour
{
    [SerializeField] private PlayerWeaponSaveSO playerWeaponSaveSo;
    private PlayerHpSystem _playerHpSystem;
    private PlayerMovement _playerMovement;
    
    #region Singleton
    public static PlayerMain Instance;
    void Awake()
    {
        Instance = this;
        
        _playerHpSystem = GetComponent<PlayerHpSystem>();
        _playerMovement = GetComponent<PlayerMovement>();
    }
    #endregion

    public PlayerMovement PlayerMovement
    {
        get => _playerMovement;
    }
    
    public PlayerHpSystem PlayerHpSystem
    {
        get => _playerHpSystem;
    }
    
    private void OnDestroy()
    {
        if (TransitionScript.IsTutorialScene()) return;
        
        TransitionScript.OnChangeLevel -= SavePlayerProgress;
    }

    
    void Start()
    {
        if (TransitionScript.IsTutorialScene()) return;     //do not allow tutorial stats to carry over
        
        TransitionScript.OnChangeLevel += SavePlayerProgress;
        
        LoadPlayerProgress();
    }
    
    private void SavePlayerProgress()
    {
        print("saving");
        ProgressSaving.RecordPlayerData(TransitionScript.lastLevelIndex, playerWeaponSaveSo.savedWepState);
        ProgressSaving.SaveFile();
    }

    private void LoadPlayerProgress()
    {
        if (ProgressSaving.LoadFile())
        {
            ProgressSaving.LoadPlayerData(playerWeaponSaveSo);
        }
    }
} 
