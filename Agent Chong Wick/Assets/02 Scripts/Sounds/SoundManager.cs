using System;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using UnityEngine.SceneManagement;

// Create the blueprint for properties the sound needs
//  System.Serializable to be accessible through the Inspector in unity


[Serializable]
public class MusicDatabase
{
    [Header("Id is based on level index to play the specific music")]
    public int sceneIndex;
    public string musicNameToPlay;
}



[Serializable]
public class EffectInfo : SoundInfo
{
    //Range helps make a slider
    [Range(.5f, .99f)] public float minRandVol = .7f;
    [Range(1.01f, 1.5f)] public float maxRandVol = 1.3f;

    [Range(.5f, .99f)] public float minRandPitch = .7f;
    [Range(1.01f, 1.5f)] public float maxRandPitch = 1.3f;
    
    public float RandomisePitch()
    {
        float newPitch = UnityEngine.Random.Range(minRandPitch, maxRandPitch);
        return newPitch;
    }

    public float RandomiseVolume()
    {
        float newVol = UnityEngine.Random.Range(minRandVol, maxRandVol);
        return newVol;
    }
}

[Serializable]
public class SoundInfo {
    
    public string name;
    public AudioClip adClip;
    
    [Range(0f, 1f)] public float volume = 0.7f;
    [Range(0f, 1f)] public float pitch = 1f;
    public bool playOnAwake;
    public bool loop;
    
    
    [HideInInspector]
    public AudioSource adSource;     // private because you dont want to expose it to other scripts (other scripts cant use it), at the same time hide it from the inspector
}



public class SoundManager : MonoBehaviour
{
    //using singleton
    public static SoundManager Instance;


    [Header("Mixer:")] 
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioMixer effectMixer;
    
    [Header("Volume:")] 
    [SerializeField] [Range(0f, 1f)] private float musicVolume = .8f;
    [SerializeField] [Range(0f, 1f)] private float effectsVolume = .8f;
    
    [Header("Music Database:")] 
    [SerializeField] private MusicDatabase[] musicDatabaseArray;

    
    [Header("Main Arrays:")]
    [SerializeField] SoundInfo[] musicsArray;
    [SerializeField] EffectInfo[] effectsArray;
    
    private AudioSource _currMusicAudioSource;

    private bool _isInBulletTime = false;
    
    public float MusicVolume
    {
        get => musicVolume;
    }

    public float EffectsVolume
    {
        get => effectsVolume;
    }

    void OnDestroy()
    {
        TransitionScript.OnSceneChange -= SwapMusic;
    }


    void Awake()
    {
        // Debug.LogError("More than one audio manager in scene");
                 // can make sure there will always be one instead of only throwing an error message;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        TransitionScript.OnSceneChange += SwapMusic;
        
        SetupArray(musicsArray, musicMixer);
        SetupArray(effectsArray, effectMixer);
        
        ChangeMusicVol(musicVolume);
        ChangeEffectVol(effectsVolume);
        
        SwapMusic(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetupArray(SoundInfo[] soundsArray, AudioMixer mixer)
    {
        foreach (SoundInfo thisSoundInfo in soundsArray)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            
            source.clip = thisSoundInfo.adClip;
            
            //settings
            source.volume = thisSoundInfo.volume;
            source.pitch = thisSoundInfo.pitch;
            source.playOnAwake = thisSoundInfo.playOnAwake;
            source.loop = thisSoundInfo.loop;
            
            source.outputAudioMixerGroup = mixer.FindMatchingGroups("Master")[0];
            
            thisSoundInfo.adSource = source;
        }
    }

    public AudioSource PlayMusic(string name)
    {
        SoundInfo foundMusic;
        for (int i = 0; i < musicsArray.Length; i++)
        {
            if (musicsArray[i].name == name)
            {
                foundMusic = musicsArray[i];
                
                StartCoroutine(FadeIn(foundMusic.adSource, 3f, 0f, foundMusic.volume));
                foundMusic.adSource.Play();
                return foundMusic.adSource;
            }
        }
        //Here == no sound found
        Debug.LogWarning($"AudioManager : Music is unavailable : Name: ({name})");
        return null;
    }
    
    //overload for more methods of using
    public AudioSource PlayMusic(AudioClip clip)
    {
        SoundInfo foundMusic;
        for (int i = 0; i < musicsArray.Length; i++)
        {
            if (musicsArray[i].adClip == clip)
            {
                foundMusic = musicsArray[i];
                
                StartCoroutine(FadeIn(foundMusic.adSource, 3f, 0f, foundMusic.volume));
                foundMusic.adSource.Play();
                return foundMusic.adSource;
            }
        }
        //Here == no sound found
        Debug.LogWarning($"AudioManager :Music is unavailable: AudioClip missing.");
        return null;
    }

    public AudioSource PlayEffect(string name, bool randomisePitch = false, bool randomiseVol = false)
    {
        EffectInfo foundEffect;
        for (int i = 0; i < effectsArray.Length; i++)
        {
            if (effectsArray[i].name == name)
            {
                foundEffect = effectsArray[i];

                float pitchDownFactor = 1f;
                if (_isInBulletTime)
                {
                    pitchDownFactor = foundEffect.adSource.pitch;
                }
                
                if (randomisePitch) foundEffect.adSource.pitch = foundEffect.RandomisePitch() * pitchDownFactor;
                if (randomiseVol) foundEffect.adSource.volume = foundEffect.RandomiseVolume();
                
                foundEffect.adSource.PlayOneShot(foundEffect.adClip);
                return foundEffect.adSource;
            }
        }
        Debug.LogWarning($"AudioManager : Effect is unavailable : Name: ({name})");
        return null;
    }
    
    //overload for more methods of using
    public AudioSource PlayEffect(AudioClip clip, bool randomisePitch = false, bool randomiseVol = false)
    {
        EffectInfo foundEffect;
        for (int i = 0; i < effectsArray.Length; i++)
        {
            if (effectsArray[i].adClip == clip)
            {
                foundEffect = effectsArray[i];

                float pitchDownFactor = 1f;
                if (_isInBulletTime)
                {
                    pitchDownFactor = foundEffect.adSource.pitch;
                }
                
                if (randomisePitch) foundEffect.adSource.pitch = foundEffect.RandomisePitch() * pitchDownFactor;
                if (randomiseVol) foundEffect.adSource.volume = foundEffect.RandomiseVolume();
                
                foundEffect.adSource.PlayOneShot(foundEffect.adClip);
                return foundEffect.adSource;
            }
        }
        //Here == no sound found
        Debug.LogWarning($"AudioManager : Effect is unavailable: AudioClip missing.");
        return null;
    }
    
    public void BulletTime_OnActivateBulletTime()
    {
        BulletTimePitchDown();
    }

    public void BulletTime_OnDeactivateBulletTime()
    {
        BulletTimePitchReset();
    }
    
    private void SwapMusic(int index)
    {
        if (_currMusicAudioSource != null)
        {
            _currMusicAudioSource.Stop();
        }


        foreach (MusicDatabase music in musicDatabaseArray)
        {
            if (music.sceneIndex == index)
            {
                _currMusicAudioSource = PlayMusic(music.musicNameToPlay);
            }
        }
    }
    
    public void ChangeMusicVol(float value)
    {
        musicMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20f);
        musicVolume = value;
    }
    
    public void ChangeEffectVol(float value)
    {   
        effectMixer.SetFloat("EffectsVolume", Mathf.Log10(value) * 20f);
        effectsVolume = value;
    }

    private void BulletTimePitchDown()
    {
        _isInBulletTime = true;
        
        foreach (EffectInfo effect in effectsArray)
        {
            AudioSource thisSource = effect.adSource;
            thisSource.pitch *= .6f;
        }

        foreach (SoundInfo music in musicsArray)
        {
            AudioSource thisSource = music.adSource;
            thisSource.pitch *= .6f;
        }
    }

    private void BulletTimePitchReset()
    {
        _isInBulletTime = false;
        
        foreach (EffectInfo effect in effectsArray)
        {
            AudioSource thisSource = effect.adSource;
            thisSource.pitch = effect.pitch;
        }
        
        foreach (SoundInfo music in musicsArray)
        {
            AudioSource thisSource = music.adSource;
            thisSource.pitch = music.pitch;
        }
    }
    
    private IEnumerator FadeIn(AudioSource source, float duration, float startValue, float targetValue)
    {
        float currTime = 0f;
        startValue = source.volume;

        while (currTime < duration)
        {
            currTime += Time.deltaTime;
            source.volume = Mathf.Lerp(startValue, targetValue, currTime / duration);

            yield return null;
        }
    }
}
