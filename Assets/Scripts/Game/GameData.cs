using Cysharp.Threading.Tasks;
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
    private GameState _state = GameState.StartMenu;
    public GameState State
    {
        get
        {
            return _state;
        }
        set
        {
            if (value == GameState.Gameplay)
            {
                Time.timeScale = 1;
                AudioListener.pause = false;
            }
            else
            {
                Time.timeScale = 0;
                AudioListener.pause = true;
            }
            GameStateChanged?.Invoke(value);
            _state = value;
        }
    }

    public async UniTask StartGame()
    {
        await PrefabCreator.Instantiate(AddressKeysConverter.Convert(AddressKeys.Char_standart) , Vector3.zero);
        GameStarted?.Invoke();
        StartGameTime = Time.time;
        State = GameState.Gameplay;
        Time.timeScale = 1;
    }

    public void AddWave()
    {
        Wave++;
    }

    public void Reset()
    {
        Wave = 0;
        //GameStateChanged = null;
    }
}
