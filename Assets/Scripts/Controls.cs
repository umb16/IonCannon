using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Controls : MonoBehaviour
{
    private UIPlayerInventory _playerInventory;
    private UIPlayerStats _playerStats;
    private GameData _gameData;
    private UIShopLayer _shop;

    [Inject]
    private void Construct(UIPlayerInventory playerInventory, UIPlayerStats playerStats, GameData gameData)
    {
        _playerInventory = playerInventory;
        _playerStats = playerStats;
        _gameData = gameData;
    }

    private void Awake()
    {
        _shop = BaseLayer.ForceGet<UIShopLayer>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_gameData.State == GameState.InShop)
            {
                _shop.Close();
            }
            else
            if (_gameData.State == GameState.Gameplay)
            {
                BaseLayer.Show<UIPlayerInventory>();
                BaseLayer.Show<UIPlayerStats>();
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
