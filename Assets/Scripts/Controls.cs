using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Controls : MonoBehaviour
{
    private UIPlayerInventory _playerInventory;
    private UIPlayerStats _playerStats;
    private GameData _gameData;
    private UIShop _shop;

    [Inject]
    private void Construct(UIPlayerInventory playerInventory, UIPlayerStats playerStats, GameData gameData, UIShop shop)
    {
        _playerInventory = playerInventory;
        _playerStats = playerStats;
        _gameData = gameData;
        _shop = shop;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_gameData.State == GameState.InShop)
            {
                _shop.OnClose();
            }
            else
            if (_gameData.State == GameState.Gameplay)
            {
                _playerInventory.Show();
                _playerStats.Show();
                _gameData.State = GameState.Inventory;
            }
            else
            if (_gameData.State == GameState.Inventory)
            {
                _playerInventory.Hide();
                _playerStats.Hide();
                _gameData.State = GameState.Gameplay;
            }
        }
    }
}
