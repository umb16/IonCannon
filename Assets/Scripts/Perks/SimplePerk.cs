using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePerk : IPerk
{
    public string Name => _name();

    public string Description => _description();

    public int Level { get; private set; }

    public int MaxLevel => _modificators.Length;
    public bool Maxed => MaxLevel == Level;

    private IStatsCollection _collection;
    private Func<string> _description;
    private Func<string> _name;
    private StatModificator[][] _modificators;
    public SimplePerk(Func<string> name, Func<string> description, StatModificator[][] statModificators, IStatsCollection statsCollection)
    {
        _collection = statsCollection;
        _name = name;
        _description = description;
        _modificators = statModificators;
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

    public void SetStatsCollection(IStatsCollection collection)
    {
        _collection = collection;
        if (Level > 0)
            _collection.AddModificators(_modificators[Level - 1]);
    }

    public void Update()
    {
        //throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        //throw new NotImplementedException();
    }

    public void OnEnemyHit(DamageSources source, IStatsCollection collection)
    {
        //throw new NotImplementedException();
    }
}
