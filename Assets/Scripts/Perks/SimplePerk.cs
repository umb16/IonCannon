using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SimplePerk : WithId, IPerk
{
    
    public PerkType Type { get; private set; }

    public bool IsCommon => true;

    private IStatsCollection _collection;
    private StatModificator[] _modificators;

    public SimplePerk(PerkType type, params StatModificator[] statModificators)
    {
        _modificators = statModificators;
        Type = type;
    }

    public void Init(IMob parent)
    {
        _collection = parent.StatsCollection;
        _collection.AddModificators(_modificators);
    }

    public void Shutdown()
    {
        _collection.RemoveModificators(_modificators);
    }

    public string GetDescription()
    {
        string result = "";
        foreach (var item in _modificators)
        {
            string text = item.ToString();
            if (!string.IsNullOrEmpty(text))
            {
                result += text+"\n";
            }
        }
        return result;
    }

    public void Add(IPerk perk)
    {
        
    }

}
