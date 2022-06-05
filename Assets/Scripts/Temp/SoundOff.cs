using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SoundOff : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _mixer;
    private AudioMixerSnapshot _mute;
    private AudioMixerSnapshot _on;
    [SerializeField] private TMP_Text _buttonText;
    private bool _isMuted = true;
    private void Start()
    {
        _on = _mixer.audioMixer.FindSnapshot("Snapshot");
        _mute = _mixer.audioMixer.FindSnapshot("MuteMusic");
        _isMuted = PlayerPrefs.GetInt("musicIsMuted", 0) == 1;
        if (_isMuted)
        {
            _mute.TransitionTo(0.0f);
            _buttonText.text = "/";
        }
        else
        {
            _on.TransitionTo(0.0f);
            _buttonText.text = "m";
        }
    }
    public void ButtonPress()
    {
        if (!_isMuted)
        {
            _mute.TransitionTo(0.0f);
            _buttonText.text = "/";
        }
        else
        {
            _on.TransitionTo(0.0f);
            _buttonText.text = "m";
        }
        _isMuted = !_isMuted;
        PlayerPrefs.SetInt("musicIsMuted", _isMuted ? 1 : 0);


    }
}
