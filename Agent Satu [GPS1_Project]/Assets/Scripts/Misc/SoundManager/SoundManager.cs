using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    #region Singleton

    public static SoundManager Instance;

    void Awake()
    {
        Instance = this;
    }
    
    #endregion
    
    
}
