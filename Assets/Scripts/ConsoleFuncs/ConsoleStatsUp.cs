using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ConsoleStatsUp : MonoBehaviour
{
    private IStatsCollection _statsCollection;
    [Inject]
    private void Construct(Player player)
    {
        _statsCollection = player.StatsCollection;
    }

    private void Start()
    {
        OnGUIConsole.Instance.AddMethod("AddStat", (Action<StatType, float>)AddStat);
    }

    private void AddStat(StatType type, float value)
    {
        Debug.Log("Add "+type+" "+value);
        var stat = _statsCollection.GetStat(type);
        stat.SetBaseValue(stat.BaseValue + value);
    }
}
