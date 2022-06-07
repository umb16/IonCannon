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
    [SerializeField] private TMP_Text _text;
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
            _text.text = "Расширено";
            return;
        }
        if (gold.Value >= _cost)
        {
            _button.interactable = true;
            _text.text = "Расширить зону\n\n" + _cost.ToString();
        }
        else
        {
            _button.interactable = false;
            _text.text = "Расширить зону\n\n<color=red>" + _cost.ToString() + "</color>";
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
