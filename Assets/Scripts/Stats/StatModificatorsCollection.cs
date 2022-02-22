using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModificatorsCollection
{
    private StatModificator[] _modificators;
    private List<IStatsCollection> _statsCollections = new List<IStatsCollection>();

    public StatModificatorsCollection(StatModificator[] modificators)
    {
        _modificators = modificators;
    }

    public void AddStatsCollection(IStatsCollection stats)
    {
        stats.AddModificators(_modificators);
        _statsCollections.Add(stats);
    }
    public void RemoveStatsCollection(IStatsCollection stats)
    {
        stats.RemoveModificators(_modificators);
        _statsCollections.Remove(stats);
    }
    public void RemoveAll()
    {
        foreach (var stats in _statsCollections)
        {
            stats.RemoveModificators(_modificators);
        }
    }
}
