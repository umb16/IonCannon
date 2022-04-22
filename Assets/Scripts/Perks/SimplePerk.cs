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
    private DamageController _damageController;
    public SimplePerk(StatModificator[] statModificators, PerkType type)
    {
        _modificators = statModificators;
        Type = type;
    }

    public void Init(IMob parent)
    {
        _collection = parent.StatsCollection;
        _damageController = parent.DamageController;
        _collection.AddModificators(_modificators);
    }

    public void Shutdown()
    {
        _collection.RemoveModificators(_modificators);
    }

    public void Add(IPerk perk)
    {
        
    }
}
