using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameData
{
    public event Action<GameState> GameStateChanged;
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
            if (value != _state && value == GameState.InGame)
            {
                StartGameTime = Time.time;
            }
            GameStateChanged?.Invoke(value);
            _state = value;
        }
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
