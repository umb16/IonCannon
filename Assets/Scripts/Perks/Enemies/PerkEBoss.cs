using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkEBoss : PerkEStandart
{
    public override PerkType Type => PerkType.EBoss;
    public int Wave => _mob.GameData.Wave;

    private IStatModificator[] _modificators;
    private Fx _aura = new Fx("Fx_AuraBoss", FxPosition.Ground);
    public override void Init(IMob mob)
    {
        base.Init(mob);
        _modificators = 
            new[] { 
                    new StatModificator((x) => x * (Wave + 1) * 6, StatModificatorType.TransformChain, StatType.Score),
                    new StatModificator((x) => (x + Wave * 10) * 17, StatModificatorType.TransformChain, StatType.MaxHP)
                    };
        _mob.StatsCollection.AddModificators(_modificators);
        mob.HP.SetBaseValue(mob.StatsCollection.GetStat(StatType.MaxHP).Value);
        _mob.AddFx(_aura);
    }

    public override void Shutdown()
    {
        _mob.RemoveFx(_aura);
        _mob.StatsCollection.RemoveModificators(_modificators);
    }
}
