using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameData
{
    public event Action<GameState> GameStateChanged;
    public event Action GameStarted;
    public int Wave = 0;
    public float StartGameTime;
    public GameState State
    {
        get
        {
            return _state;
        }
        set
        {
            if (value == GameState.Gameplay)
                Time.timeScale = 1;
            else
                Time.timeScale = 0;
            GameStateChanged?.Invoke(value);
            _state = value;
        }
    }

    public void StartGame()
    {
        GameStarted?.Invoke();
        StartGameTime = Time.time;
        State = GameState.Gameplay;
    }

    public void AddWave()
    {
        Wave++;
    }

    public void Reset()
    {
        Wave = 0;
        GameStateChanged = null;
    }

    private GameState _state = GameState.StartMenu;
}
