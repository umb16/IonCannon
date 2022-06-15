using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundsMenuLayer : BaseLayer
{
    [SerializeField] AudioMixer _mixer;
    
    public void OnMusicVolumeChanged(float volume)
    {
        _mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }
    public void OnSoundVolumeChanged(float volume)
    {
        _mixer.SetFloat("SoundVolume", Mathf.Log10(volume) * 20);
    }
}
