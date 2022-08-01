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
        _gameData.GameStateChanged += GameStateChanged;
    }

    private void GameStateChanged(GameState state)
    {
        if (state == GameState.Restart)
            text.text = "";
    }

    private void OnDestroy()
    {
        _gameData.GameStateChanged -= GameStateChanged;
    }

    private void Update()
    {
        if (_gameData.State == GameState.Gameplay)
        {
            var time = TimeSpan.FromSeconds(_gameData.LastGameTime);
            if (time.Hours > 0)
                text.text = time.ToString(@"hh\:mm\:ss");
            else
                text.text = time.ToString(@"mm\:ss");
        }
    }
}
