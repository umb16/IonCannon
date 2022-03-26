using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ConsoleMethods : MonoBehaviour
{
    private IStatsCollection _statsCollection;
    private MobSpawner _mobSpawner;

    [Inject]
    private void Construct(Player player, MobSpawner mobSpawner)
    {
        _statsCollection = player.StatsCollection;
        _mobSpawner = mobSpawner;
    }

    private void Start()
    {
        OnGUIConsole.Instance.AddMethod("AddStat", (Action<StatType, float>)AddStat);
        OnGUIConsole.Instance.AddMethod("SpawnerStop", (Action<int>)SpawnerStop);
    }

    private void AddStat(StatType type, float value)
    {
        Debug.Log("Add "+type+" "+value);
        var stat = _statsCollection.GetStat(type);
        stat.SetBaseValue(stat.BaseValue + value);
    }

    private void SpawnerStop(int stop)
    {
        _mobSpawner.Stop = stop > 0;
    }
}
