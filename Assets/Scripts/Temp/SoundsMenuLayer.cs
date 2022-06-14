using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundsMenuLayer : BaseLayer
{
    [SerializeField] AudioMixer _mixer;
    // Start is called before the first frame update
    public void OnMusicVolumeChanged(float volume)
    {
        _mixer.SetFloat("MusicVolume", Mathf.Log(volume) * 20);
    }
    public void OnSoundVolumeChanged(float volume)
    {
        _mixer.SetFloat("SoundVolume", Mathf.Log(volume) * 20);
    }
}
