using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SaveLoad : MonoBehaviour
{
    [SerializeField] AudioMixer _mixer;
    void Start()
    {
        _mixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("music",1)) * 20);
    }
    public void OnSoundVolumeChanged(float volume)
    {
        _mixer.SetFloat("SoundVolume", Mathf.Log10(PlayerPrefs.GetFloat("sound", 1)) * 20);
        _mixer.SetFloat("OtherSoundsVolume", Mathf.Log10(PlayerPrefs.GetFloat("sound", 1)) * 20);
    }
}
