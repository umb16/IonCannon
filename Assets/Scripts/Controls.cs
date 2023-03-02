using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Controls : MonoBehaviour
{
    private GameData _gameData;
    private UIStates _oldUIStatus;

    [Inject]
    private void Construct(GameData gameData)
    {
        _gameData = gameData;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote) && Application.isEditor && _gameData.UIStatus != UIStates.Console)
        {
            _oldUIStatus = _gameData.UIStatus;
            _gameData.UIStatus = UIStates.Console;
        }
        if(Input.GetKeyDown(KeyCode.Escape) && Application.isEditor && _gameData.UIStatus == UIStates.Console)
        {
            _gameData.UIStatus = _oldUIStatus;
            return;
        }
        if (_gameData.UIStatus == UIStates.Console)
            return;
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q) && !CheatPanelLayer.Enabled)
        {
            if (_gameData.State == GameState.InShop)
            {
                _gameData.State = GameState.Gameplay;
                _gameData.UIStatus = UIStates.Play;
            }
            else
            if (_gameData.State == GameState.Gameplay)
            {
                _gameData.UIStatus = UIStates.Pause;
                _gameData.State = GameState.Inventory;
            }
            else
            if (_gameData.State == GameState.Inventory)
            {
                _gameData.State = GameState.Gameplay;
                _gameData.UIStatus = UIStates.Play;
            }
        }
    }
}
