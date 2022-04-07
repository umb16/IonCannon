using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkEBoss : PerkEStandart
{
    public override PerkType Type => PerkType.EBoss;
    public int Wave => _mob.GameData.Wave;

    private StatModificatorsCollection _modificators;

    public override void Init(IMob mob)
    {
        base.Init(mob);
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

    public override void Shutdown()
    {
        _modificators.RemoveStatsCollection(_mob.StatsCollection);
    }
}
