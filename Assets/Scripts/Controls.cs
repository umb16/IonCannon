using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Controls : MonoBehaviour
{
    private GameData _gameData;

    [Inject]
    private void Construct(GameData gameData)
    {
        _gameData = gameData;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote) && Application.isEditor && _gameData.UIStatus != UIStates.Console)
        {
            _gameData.SetState(GameState.Console);
        }
        if(Input.GetKeyDown(KeyCode.Escape) && Application.isEditor && _gameData.UIStatus == UIStates.Console)
        {
            _gameData.ReturnToPrevStatus();
            return;
        }
        if (_gameData.UIStatus == UIStates.Console)
            return;
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q) && !CheatPanelLayer.Enabled)
        {
            if (_gameData.Status == GameState.InShop || _gameData.Status == GameState.Inventory)
            {
                _gameData.SetState(GameState.Gameplay);
            }
            else
            if (_gameData.Status == GameState.Gameplay)
            {
                _gameData.SetState(GameState.Inventory);
            }
        }
    }
}
