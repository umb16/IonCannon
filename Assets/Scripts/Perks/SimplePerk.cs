using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePerk : IPerk
{
    public PerkType Type { get; private set; }
    public string Name => _name();

    public string Description => _description();

    public int Level { get; private set; }

    public int MaxLevel => _modificators.Length;
    public bool Maxed => MaxLevel == Level;

    private IStatsCollection _collection;
    private Func<string> _description;
    private Func<string> _name;
    private StatModificator[][] _modificators;
    private DamageController _damageController;
    public SimplePerk(Func<string> name, Func<string> description, StatModificator[][] statModificators, IMob mob, PerkType type)
    {
        _name = name;
        _description = description;
        _modificators = statModificators;
        Type = type;
        SetParent(mob);
    }
    public void AddLevel()
    {
        if (Level > 0)
            _collection.RemoveModificators(_modificators[Level - 1] );
        Level++;
        _collection.AddModificators( _modificators[Level - 1] );
    }

    public void SetLevel(int level)
    {
        if (Level > 0)
            _collection.RemoveModificators( _modificators[Level - 1] );
        Level = level;
        _collection.AddModificators(_modificators[Level - 1]);
    }

    public void SetParent(IMob parent)
    {
        _collection = parent.StatsCollection;
        _damageController = parent.DamageController;
        if (Level > 0)
            _collection.AddModificators(_modificators[Level - 1]);
    }

    public void Shutdown()
    {
        //throw new NotImplementedException();
    }
}
