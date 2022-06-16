using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundsMenuLayer : BaseLayer
{
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _soundSlider;
    [SerializeField] AudioMixer _mixer;

    private void Start()
    {
        _musicSlider.value = PlayerPrefs.GetFloat("music", 1);
        _soundSlider.value = PlayerPrefs.GetFloat("sound", 1);
    }
    public void OnMusicVolumeChanged(float volume)
    {
        _mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("music", volume);
    }
    public void OnSoundVolumeChanged(float volume)
    {
        _mixer.SetFloat("SoundVolume", Mathf.Log10(volume) * 20);
        _mixer.SetFloat("OtherSoundsVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sound", volume);
    }
}
