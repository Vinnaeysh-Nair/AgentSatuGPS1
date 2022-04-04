using UnityEngine;

// Create the blueprint for properties the sound needs
//System.serializble to be accesible through the properties tab in unity

[System.Serializable]
public class SoundInfo {
    public string Name;

    //Range helps make a slider
    [Range(0f,1f)]
    public float Volume = 0.7f;
    [Range(0.5f, 1-5f)]
    public float Pitch = 1f;

    [Range(0f, 0.5f)]
    public float randomVol = 0.1f;
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;

    public AudioClip AdClip;
    // private because you want to acces it through a method
    private AudioSource AdSource;

    public void SetSource(AudioSource _source)
    {
        AdSource = _source;
        AdSource.clip = AdClip;
    }

    public void Play() {

        AdSource.volume = Volume * (1 +Random.Range(-randomVol / 2f , randomVol / 2f));
        AdSource.pitch = Pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        AdSource.Play();
        
    }
}

public class Soundmanager : MonoBehaviour
{
    //using singleton
    public static Soundmanager instance;

    [SerializeField] SoundInfo[] sounds;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one audio manager in scene");
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].Name);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }

        PlaySound("BackgroundSound");
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].Name == _name)
            {
                sounds[i].Play();
                return;
            }
        }

        //Here == no sound found
        Debug.LogWarning("AudioManager : Sound unavailable : " +_name);
    }
}
