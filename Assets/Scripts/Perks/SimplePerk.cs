using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SimplePerk : PerkCommonBase
{
    private IStatsCollection _collection;
    private StatModificator[] _modificators;

    public SimplePerk(PerkType type, params StatModificator[] statModificators)
    {
        _modificators = statModificators;
        SetType(type);
    }

    public override void Init(IMob parent)
    {
        _collection = parent.StatsCollection;
        _collection.AddModificators(_modificators);
    }

    public override void Shutdown()
    {
        _collection.RemoveModificators(_modificators);
    }

    public override string GetDescription()
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
}
