using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] public AudioMixer Mixer;
    [SerializeField] private AudioSource _audioSourceIgnoreLowpass;
    
    //Ignore
    [SerializeField] private AudioClip _shopOpen;
    [SerializeField] private AudioClip _shopClose;
    [SerializeField] private AudioClip _transmutate;
    [SerializeField] private AudioClip _changeSlot;

    [SerializeField] private AudioSource _audioSourceGameplay;

    [SerializeField] private AudioClip _rayReady;

    private void Awake()
    {
        Instance = this;
        _audioSourceIgnoreLowpass.ignoreListenerPause = true;
    }
    public void PlayShopOpen()
    {
        _audioSourceIgnoreLowpass.PlayOneShot(_shopOpen);
    }
    public void PlayChangeSlot()
    {
        _audioSourceIgnoreLowpass.PlayOneShot(_changeSlot);
    }
    public void PlayShopClose()
    {
        _audioSourceIgnoreLowpass.PlayOneShot(_shopClose);
    }
    public void PlayTransmutate()
    {
        _audioSourceIgnoreLowpass.PlayOneShot(_transmutate);
    }
    public void PlayRayReady()
    {
        _audioSourceIgnoreLowpass?.PlayOneShot(_rayReady);
    }

    public void Play(AudioClip clip)
    {
        _audioSourceGameplay.PlayOneShot(clip);
    }
}
