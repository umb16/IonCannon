using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class TimeUI : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    private GameData _gameData;

    [Inject]
    private void Construct(GameData gameData)
    {
        _gameData = gameData;
        _gameData.GameStarted += GameStarted;
    }

    private void GameStarted()
    {
        text.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        _gameData.GameStarted -= GameStarted;
    }

    private void Update()
    {
        if (_gameData.State == GameState.Gameplay)
        {
            var time = TimeSpan.FromSeconds(Time.time - _gameData.StartGameTime);
            if (time.Hours > 0)
                text.text = time.ToString(@"hh\:mm\:ss");
            else
                text.text = time.ToString(@"mm\:ss");
        }
    }
}
