using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIPlayerStats : BaseLayer
{
    [SerializeField] GameObject _textPrefab;
    private Player _player;

    [Inject]
    private void Construct(Player player)
    {
        _player = player;
        AddString("Максимальное здоровье", StatType.MaxHP);
        AddString("Скорость бега", StatType.MovementSpeed," п/с");
        AddString("Радиус сбора", StatType.PickupRadius," п");
        AddString("Урон луча", StatType.RayDamage);
        AddString("Скорость луча", StatType.RaySpeed," п/с");
        AddString("Длинна пути луча", StatType.RayPathLenght," п");
        AddString("Время наведения луча", StatType.RayDelay," c");
        AddString("Ширина луча", StatType.RayDamageAreaRadius," п");
        AddString("Погрешность", StatType.RayError," п");
    }

    private void AddString(string text, StatType statType, string postfix="", string format="")
    {
        var maxHPText = AddString();
        maxHPText.SetText(text);
        var stat = _player.StatsCollection.GetStat(statType);
        maxHPText.SetValue(stat.Value.ToString(format)+postfix);
        stat.ValueChanged += x => maxHPText.SetValue(stat.Value.ToString(format) + postfix);
    }

    private void AddString(string text)
    {
        AddString().SetText(text);
    }

    private UIStatText AddString()
    {
        var go = Instantiate(_textPrefab, _textPrefab.transform.parent);
        go.SetActive(true);
        return go.GetComponent<UIStatText>();
    }
}
