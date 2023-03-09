using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using MiniScriptSharp.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameData
{
    public event Action<GameState> GameStateChanged;
    public event Action<int> WaveChanged;
    public event Action GameStarted;
    public UIStates UIStatus { get; private set; }
    public int Wave = 0;
    public float StartGameTime;

    private Timer _timer = null;
    public float GameTime => Time.time - StartGameTime;
    public float LastGameTime => 960 -( Time.time - StartGameTime);
    public GameState Status { get; private set; } = GameState.StartMenu;

    private GameState _oldState;

    public void SetState(GameState state)
    {
        switch (state)
        {
            case GameState.StartMenu:
                UIStatus = UIStates.StartMenu;
                break;
            case GameState.Restart:
                break;
            case GameState.Gameplay:
                UIStatus = UIStates.Gameplay;
                break;
            case GameState.GameOver:
                UIStatus = UIStates.GameOver;
                break;
            case GameState.InShop:
                UIStatus = UIStates.Shop;
                break;
            case GameState.Inventory:
                UIStatus = UIStates.EscMenu;
                break;
            case GameState.Lobby:
                UIStatus = UIStates.Lobby;
                break;
            case GameState.Console:
                UIStatus = UIStates.Console;
                break;
            default:
                break;
        }

        if (state == GameState.GameOver)
        {
            _timer?.Stop();
            _timer = new Timer(.5f).SetUpdate(x => Time.timeScale = 1 - x);
        }
        else
        {
            _timer?.Stop();
            if (state == GameState.Gameplay)
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
        GameStateChanged?.Invoke(state);
        _oldState = Status;
        Status = state;
    }

    public void ReturnToPrevStatus()
    {
        SetState(_oldState);
    }

    int charIndexer = 0;
    public async UniTask StartGame(string charName)
    {
        //_currentCharName = charName;
        var Char = await PrefabCreator.Instantiate(charName, Vector3.zero);
        Char.transform.eulerAngles -= Vector3.right * 90;
        Char.name = "char " + (charIndexer++);
        GameStarted?.Invoke();
        StartGameTime = Time.time;
        Status = GameState.Gameplay;
        Time.timeScale = 1;
        UIStatus = UIStates.Gameplay;
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
