using System;
using Unity.VisualScripting;
using UnityEngine;



public class SoundManager : MonoBehaviour
{
    //[SerializeField] private AudioSource musicSource, soundSource;

    // [SerializeField] private Sound[] music, sound;
    //
    // public class Audio
    // {
    //     public AudioSource source;
    //     public Clip[] clips;
    //     
    //     public bool loop;
    //     public bool playOnAwake;
    //     
    //     [Range(0f, 1f)] public float volume;
    // }
    //
    // public struct Clip
    // {
    //     public string name;
    //     public AudioClip[] clip;
    // }
    //
    //
    
    
    public Sound[] soundsArray;
    
    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public bool loop;
        public bool playOnAwake;
        
        [HideInInspector]
        public AudioSource source;
    
        [Range(0f, 1f)] public float volume;
    }
    
    #region Singleton
    
    public static SoundManager Instance;
   
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }
   #endregion
    
    void Start()
    {
        foreach (Sound s in soundsArray)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(soundsArray, s => s.name == name);
        s.source.PlayOneShot(s.clip);
    }
    

    // public void PlayMusic(string name)
    // {
    //   //  musicSource.PlayOneShot(clip);
    //
    //   foreach (Sound s in music)
    //   {
    //       
    //   }
    // }
    //
    // public void PlaySound(string name)
    // {
    //     soundSource.PlayOneShot(clip);
    // }
}
