using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;

public class UIWave : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    private GameData _gameData;
    private int _wave;
    private LocalizedString _waveLocal = LocaleKeys.Main.WAVE; 

    [Inject]
    private void Construct(GameData gameData)
    {
        _gameData = gameData;
        _gameData.OnReset += ()=> _text.text = "";
        _gameData.WaveChanged += WaveChanged;
        _waveLocal.StringChanged += UpdateText;
    }

    private void WaveChanged(int wave)
    {
       _wave = wave;
        UpdateText(_waveLocal.GetLocalizedString());
    }

    private void UpdateText(string text)
    {
        _text.text = text + ": " + (_wave+1);
    }
}
