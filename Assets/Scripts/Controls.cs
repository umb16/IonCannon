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

    private void Awake()
    {
       /* _playerStats = BaseLayer.ForceGet<UIPlayerStats>();
        _playerInventory = BaseLayer.ForceGet<UIPlayerInventory>();
        _soundsMenu = BaseLayer.ForceGet<SoundsMenuLayer>();

        _shop = BaseLayer.ForceGet<UIShopLayer>();*/
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote) && Application.isEditor)
        {
            _gameData.UIStatus = UIStates.Console;
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q) && !CheatPanelLayer.Enabled)
        {
            if (_gameData.State == GameState.InShop)
            {
                //_shop.Close();
                _gameData.State = GameState.Gameplay;
                _gameData.UIStatus = UIStates.Play;
            }
            else
            if (_gameData.State == GameState.Gameplay)
            {
                //BaseLayer.Show<UIShopBack>();
                //BaseLayer.Show<UIPlayerInventory>();
                //BaseLayer.Show<UIPlayerStats>();
                //BaseLayer.Show<SoundsMenuLayer>();
                _gameData.UIStatus = UIStates.Pause;
                _gameData.State = GameState.Inventory;
            }
            else
            if (_gameData.State == GameState.Inventory)
            {
                //_playerInventory.Hide();
                //_playerStats.Hide();
                //_soundsMenu.Hide();
                _gameData.State = GameState.Gameplay;
                _gameData.UIStatus = UIStates.Play;
            }
        }
    }
}
