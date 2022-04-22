using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkEChampion : PerkEStandart
{
    public override PerkType Type => PerkType.EChampion;

    private IStatModificator[] _modificators = new[] {
                    new StatModificator(0.5f, StatModificatorType.Multiplicative, StatType.Score),
                    new StatModificator(0.5f, StatModificatorType.Multiplicative, StatType.HP),
                    new StatModificator(0.5f, StatModificatorType.Multiplicative, StatType.MaxHP),
                  };

    public override void Init(IMob mob)
    {
        base.Init(mob);
        _mob.StatsCollection.AddModificators(_modificators);
    }
    public override void Shutdown()
    {
        base.Shutdown();
        _mob.StatsCollection.RemoveModificators(_modificators);
    }
}
