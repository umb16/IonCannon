using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameData
{
    public event Action<GameState> GameStateChanged;
    public event Action<int> WaveChanged;
    public event Action GameStarted;
    public int Wave = 0;
    public float StartGameTime;
    private GameState _state = GameState.StartMenu;
    private Timer _timer = null;
    public float GameTime => Time.time - StartGameTime;
    public GameState State
    {
        get
        {
            return _state;
        }
        set
        {
            if (value == GameState.GameOver)
            {
                _timer?.Stop();
                _timer = new Timer(.5f).SetUpdate(x => Time.timeScale = 1 - x);
                //AudioListener.pause = true;
            }
            else
            {
                _timer?.Stop();
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
            }
            GameStateChanged?.Invoke(value);
            _state = value;
        }
    }

    int charIndexer = 0;
    public async UniTask StartGame()
    {
        var Char = await PrefabCreator.Instantiate(Addresses.Char_standart, Vector3.zero);
        Char.name = "char " + (charIndexer++);
        GameStarted?.Invoke();
        StartGameTime = Time.time;
        State = GameState.Gameplay;
        Time.timeScale = 1;
    }

    public void AddWave()
    {
        Wave++;
        WaveChanged?.Invoke(Wave);
    }

    public void Reset()
    {
        Wave = 0;
        WaveChanged?.Invoke(Wave);
        //GameStateChanged = null;
    }
}
