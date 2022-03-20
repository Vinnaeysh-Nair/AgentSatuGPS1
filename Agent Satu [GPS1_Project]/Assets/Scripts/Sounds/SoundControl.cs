using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : MonoBehaviour
{
    private AudioSource audioSrc;

    [SerializeField] private AudioClip sound;
    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    public void PlaySound() 
    {
        audioSrc.PlayOneShot(sound);
    }
}
