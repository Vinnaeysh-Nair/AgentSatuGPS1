using UnityEngine;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    [Header("Volume Sliders")] 
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;

    private void OnDestroy()
    {
        SoundManager sm = SoundManager.Instance;
        
        musicSlider.onValueChanged.RemoveListener(sm.ChangeMusicVol);
        effectsSlider.onValueChanged.RemoveListener(sm.ChangeEffectVol);
    }

    void Start()
    {
        SetVolumeSliderValues();
        
        SoundManager sm = SoundManager.Instance;
        musicSlider.onValueChanged.AddListener(sm.ChangeMusicVol);
        effectsSlider.onValueChanged.AddListener(sm.ChangeEffectVol);
    }
    
    private void SetVolumeSliderValues()
    {
        SoundManager sm = SoundManager.Instance;

        musicSlider.value = sm.MusicVolume;
        effectsSlider.value = sm.EffectsVolume;
    }
}