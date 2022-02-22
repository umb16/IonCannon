using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkEChampion : IPerk
{
    public string Name => throw new System.NotImplementedException();

    public string Description => throw new System.NotImplementedException();

    private Mob _mob;

    public int Level => 1;

    public bool Maxed => true;

    public int MaxLevel => 1;

    private StatModificatorsCollection _modificators;

    public PerkEChampion(Mob mob)
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
                    new StatModificator(1.5f, StatModificatorType.Multiplicative, StatType.Score),
                    new StatModificator(1.5f, StatModificatorType.Multiplicative, StatType.HP),
                    new StatModificator(1.5f, StatModificatorType.Multiplicative, StatType.MaxHP),
                    new StatModificator(1.5f, StatModificatorType.Multiplicative, StatType.Size),
                    }
        );
        _modificators.AddStatsCollection(_mob.StatsCollection);
    }
}
