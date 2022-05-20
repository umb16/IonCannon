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
        AddString("������������ ��������", StatType.MaxHP);
        AddString("�������� ����", StatType.MovementSpeed," �/�");
        AddString("������ �����", StatType.PickupRadius," �");
        AddString("���� ����", StatType.RayDamage);
        AddString("�������� ����", StatType.RaySpeed," �/�");
        AddString("������ ���� ����", StatType.RayPathLenght," �");
        AddString("����� ��������� ����", StatType.RayDelay," c");
        AddString("������ ����", StatType.RayDamageAreaRadius," �");
        AddString("�����������", StatType.RayError," �");
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
