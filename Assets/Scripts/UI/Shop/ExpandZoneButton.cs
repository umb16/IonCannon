using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using SPVD.LifeSupport;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ExpandZoneButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextWithIcon _costText;
    [SerializeField] private GameObject _normalText;
    [SerializeField] private GameObject _expandedText;
    private AsyncReactiveProperty<Player> _player;
    private LifeSupportTower _lifeSupportTower;
    private ShopShip _ship;
    private int _cost = 100;
    private bool _once = false;
    [Inject]
    private void Construct(LifeSupportTower lifeSupportTower, ShopShip ship, AsyncReactiveProperty<Player> player,
        GameData gameData)
    {
        _lifeSupportTower = lifeSupportTower;
        _ship = ship;
        _player = player;
        player.Where(x => x != null).ForEachAsync(x => x.Gold.ValueChanged += CheckButtonStatus);
        _button.onClick.AddListener(OnExpandButtonClick);
        gameData.GameStateChanged += GameStateChanged;
    }

    private void OnEnable()
    {
        _once = false;
        if (_player.Value != null)
            CheckButtonStatus(_player.Value.Gold);
    }

    private void GameStateChanged(GameState state)
    {
        if (state == GameState.Restart)
        {
            _cost = 100;
        }
    }

    private void CheckButtonStatus(ComplexStat gold)
    {
        if (_once)
        {
            _button.interactable = false;
            _normalText.SetActive(false);
            _expandedText.SetActive(true);
            _costText.Hide();
            return;
        }
        _normalText.SetActive(true);
        _expandedText.SetActive(false);
        _costText.Show();
        if (gold.Value >= _cost)
        {
            _button.interactable = true;
            _costText.SetText(_cost.ToString());
        }
        else
        {
            _button.interactable = false;
            _costText.SetText("<color=red>" + _cost.ToString() + "</color>");
        }
    }

    private void OnExpandButtonClick()
    {
        _lifeSupportTower.AddCircle((Vector2)_ship.transform.position, 20);
        _once = true;
        _player.Value.Gold.AddBaseValue(-_cost);
        _cost *= 2;
    }
}
