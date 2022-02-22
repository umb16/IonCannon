using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkEBoss : IPerk
{
    public string Name => throw new System.NotImplementedException();

    public string Description => throw new System.NotImplementedException();

    private Mob _mob;

    public int Level => 1;

    public bool Maxed => true;

    public int MaxLevel => 1;
    public int Wave => _mob.GameData.Wave;

    private StatModificatorsCollection _modificators;

    public PerkEBoss(Mob mob)
    {
        SetParent(mob);
    }
    public void AddLevel()
    {
        Debug.Log("Is static perk");
    }

    public void SetLevel(int level)
    {
        Debug.Log("Is static perk");
    }

    public void SetParent(Mob mob)
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
                    new StatModificator((x) => (x + Wave * 10) * 17, StatModificatorType.TransformChain, StatType.MaxHP),
                    new StatModificator(2, StatModificatorType.Multiplicative, StatType.Size),
                    }
        );
        _modificators.AddStatsCollection(_mob.StatsCollection);
    }
}
