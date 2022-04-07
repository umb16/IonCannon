using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameData
{
    public event Action<GameState> GameStateChanged;
    public int Wave = 0;
    public GameState State
    {
        get
        {
            return _state;
        }
        set
        {
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
