using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandartStatsCollection : IStatsCollection
{
    Dictionary<StatType, ComplexStat> _stats = new Dictionary<StatType, ComplexStat>();
    public StandartStatsCollection((StatType type, ComplexStat stat)[] stats)
    {
        foreach (var stat in stats)
        {
            _stats.Add(stat.type, stat.stat);
        }
    }
    public void AddModificators(StatModificator[] modificators)
    {
        foreach (var mod in modificators)
        {
            _stats[mod.StatType].AddModificator(mod);
        }
    }

    public ComplexStat GetStat(StatType statType)
    {
        if (_stats.TryGetValue(statType, out ComplexStat result))
            return result;
        Debug.LogWarning("GetStat: stat not found " + statType);
        var stat = new ComplexStat(0);
        _stats[statType] = stat;
        return stat;
    }

    public void RemoveModificators(StatModificator[] modificators)
    {
        foreach (var mod in modificators)
        {
            _stats[mod.StatType].RemoveModificator(mod);
        }
    }

    public void SetStat(StatType statType, float value)
    {
        if (_stats.ContainsKey(statType))
        {
            _stats[statType].SetBaseValue(value);
        }
        else
        {
            var stat = new ComplexStat(value);
            _stats[statType] = stat;
            Debug.LogWarning("SetStat: " + statType + " not found");
        }
    }
}
