using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkEChampion : PerkEStandart
{
    public override PerkType Type => PerkType.EChampion;

    private StatModificatorsCollection _modificators;

    public override void Init(IMob mob)
    {
        base.Init(mob);
        _modificators = new StatModificatorsCollection
        (
            new[] { 
                    new StatModificator(0.5f, StatModificatorType.Multiplicative, StatType.Score),
                    new StatModificator(0.5f, StatModificatorType.Multiplicative, StatType.HP),
                    new StatModificator(0.5f, StatModificatorType.Multiplicative, StatType.MaxHP),
                  }
        );
        _modificators.AddStatsCollection(_mob.StatsCollection);
    }
}
