using UnityEngine;

public  class PlayerMain : MonoBehaviour
{
    [SerializeField] private PlayerWeaponSaveSO playerWeaponSaveSo;

    public static PlayerMain Instance;


    private void OnDestroy()
    {
        TransitionScript.OnChangeLevel -= SavePlayerProgress;
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        TransitionScript.OnChangeLevel += SavePlayerProgress;
        
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
