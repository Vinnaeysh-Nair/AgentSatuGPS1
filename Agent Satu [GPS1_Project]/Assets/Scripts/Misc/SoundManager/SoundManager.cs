using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;



public class SoundManager : MonoBehaviour
{
    #region Singleton

    public static SoundManager Instance;

    void Awake()
    {
        Instance = this;
    }
    
    #endregion

    public List<Sound> soundsList;

    [System.Serializable]
    public class Sound
    {
        [SerializeField] private string name;
        public AudioSource source;
        public AudioClip[] clip;
    }
        
    void PlaySound()
    {
        
    }
    
}
