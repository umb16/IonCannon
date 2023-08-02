using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardStatsCollection : IStatsCollection
{
    Dictionary<StatType, ComplexStat> _stats = new Dictionary<StatType, ComplexStat>();
    public StandardStatsCollection((StatType type, ComplexStat stat)[] stats)
    {
        foreach (var stat in stats)
        {
            _stats.Add(stat.type, stat.stat);
        }
    }
    public void AddModificators(IStatModificator[] modificators)
    {
        foreach (var mod in modificators)
        {
            _stats[mod.StatType].AddModificator(mod);
        }
    }
    public void RemoveModificators(IStatModificator[] modificators)
    {
        foreach (var mod in modificators)
        {
            _stats[mod.StatType].RemoveModificator(mod);
        }
    }
    public void AddModificator(IStatModificator modificator)
    {
        GetStat(modificator.StatType).AddModificator(modificator);
    }
    public void RemoveModificator(IStatModificator modificator)
    {
        GetStat(modificator.StatType).RemoveModificator(modificator);
    }

    public ComplexStat GetStat(StatType statType)
    {
        if (_stats.TryGetValue(statType, out ComplexStat result))
            return result;
#if STATS_WARNING
        Debug.LogWarning("GetStat: stat not found " + statType);
#endif
        var stat = new ComplexStat(0);
        _stats[statType] = stat;
        return stat;
    }



    public void SetStat(StatType statType, float value)
    {
        if (_stats.ContainsKey(statType))
        {
            _stats[statType].SetBaseValue(value);
        }
        else
        {
            var newStat = new ComplexStat(value);
            _stats[statType] = newStat;
            Debug.LogWarning("SetStat: " + statType + " not found");
        }
    }

    public void AddStat(StatType statType, float value)
    {
        if (_stats.TryGetValue(statType, out var stat))
        {
            _stats[statType].SetBaseValue(stat.BaseValue + value);
        }
        else
        {
            var newStat = new ComplexStat(value);
            _stats[statType] = newStat;
            Debug.LogWarning("SetStat: " + statType + " not found");
        }
    }
}
