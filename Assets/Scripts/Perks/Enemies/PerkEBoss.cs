using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkEBoss : IPerk
{
    public PerkType Type => PerkType.EBoss;
    public string Name => throw new System.NotImplementedException();

    public string Description => throw new System.NotImplementedException();

    private IMob _mob;

    public int Level => 1;

    public bool Maxed => true;

    public int MaxLevel => 1;
    public int Wave => _mob.GameData.Wave;

    private StatModificatorsCollection _modificators;

    public void AddLevel()
    {
        Debug.Log("Is static perk");
    }

    public void SetLevel(int level)
    {
        Debug.Log("Is static perk");
    }

    public void Init(IMob mob)
    {
        if (mob == null)
        {
            _modificators.RemoveStatsCollection(_mob.StatsCollection);
        }
        _mob = mob;
        _modificators = new StatModificatorsCollection
        (
            new[] { 
                    new StatModificator((x) => x * (Wave + 1) * 6, StatModificatorType.TransformChain, StatType.Score),
                    new StatModificator((x) => (x + Wave * 10) * 17, StatModificatorType.TransformChain, StatType.HP),
                    new StatModificator((x) => (x + Wave * 10) * 17, StatModificatorType.TransformChain, StatType.MaxHP)
                    }
        );
        _modificators.AddStatsCollection(_mob.StatsCollection);
    }

    public void Shutdown()
    {
        _modificators.RemoveStatsCollection(_mob.StatsCollection);
    }

    public void Add(IPerk perk)
    {
        
    }
}
