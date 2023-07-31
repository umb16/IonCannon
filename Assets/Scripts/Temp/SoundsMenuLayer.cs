using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Zenject;

public class SoundsMenuLayer : MonoBehaviour
{
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _soundSlider;
    AudioMixer _mixer;
    private GameData _gameData;

    [Inject]
    private void Construct(GameData gameData)
    {
        _gameData = gameData;
    }

    private void Start()
    {
        _mixer = SoundManager.Instance.Mixer;
        _musicSlider.value = PlayerPrefs.GetFloat("music", 1);
        _soundSlider.value = PlayerPrefs.GetFloat("sound", 1);
        _musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        _soundSlider.onValueChanged.AddListener(OnSoundVolumeChanged);
    }
    public void OnMusicVolumeChanged(float volume)
    {
        _mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20 - 9);
        PlayerPrefs.SetFloat("music", volume);
    }
    public void OnSoundVolumeChanged(float volume)
    {
        _mixer.SetFloat("SoundVolume", Mathf.Log10(volume) * 20 - 6);
        _mixer.SetFloat("OtherSoundsVolume", Mathf.Log10(volume) * 20 - 6);
        PlayerPrefs.SetFloat("sound", volume);
    }
    public void GoToMainMenu()
    {
        _gameData.Reset();
        _gameData.SetState(GameState.StartMenu);
    }
}
