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

public class ExpandInventoryButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _text;
    private AsyncReactiveProperty<Player> _player;
    private int _cost = 100;
    private GameData _gameData;

    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player,
        GameData gameData)
    {
        _player = player;
        player.Where(x => x != null).ForEachAsync(x =>
        {
            CheckButtonStatus(_player.Value.Gold);
            x.Gold.ValueChanged += CheckButtonStatus;
        });
        _button.onClick.AddListener(OnExpandButtonClick);
        gameData.GameStateChanged += GameStateChanged;
        gameData.OnReset += () => _cost = 100;
        _gameData = gameData;
    }

    private void GameStateChanged(GameState state)
    {
        if (state == GameState.InShop && _cost <1600)
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }
        else
            gameObject.SetActive(false);
    }

    private void CheckButtonStatus(ComplexStat gold)
    {
        if (gold.Value >= _cost)
        {
            _button.interactable = true;
            _text.text = _cost.ToString();
        }
        else
        {
            _button.interactable = false;
            _text.text = "<color=red>" + _cost.ToString() + "</color>";
        }
    }

    private void OnExpandButtonClick()
    {
        _player.Value.Inventory.AddSlot();
        _player.Value.Stash.AddSlot();
        //������� ����� ��������� ���������� �������� ������
        _cost *= 2;
        _player.Value.Gold.AddBaseValue(-_cost / 2);
        if (_cost > 1600)
            gameObject.SetActive(false);
        transform.SetAsLastSibling();
    }
}
