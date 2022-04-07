using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkEWave : PerkEStandart
{
    public override PerkType Type => PerkType.EWaveFactor;

    public int Wave => _mob.GameData.Wave;

    private StatModificatorsCollection _modificators;

    public override async void Init(IMob mob)
    {
        await UniTask.WaitUntil(() => mob.IsReady);
        base.Init(mob);
        _modificators = new StatModificatorsCollection
        (
            new[] { 
                    new StatModificator((x) => x * (Wave + 1), StatModificatorType.TransformChain, StatType.Score),
                    new StatModificator((x) => x * (Wave + 1), StatModificatorType.TransformChain, StatType.HP),
                    new StatModificator((x) => x * (Wave + 1), StatModificatorType.TransformChain, StatType.MaxHP),
                  }
        );
        _modificators.AddStatsCollection(_mob.StatsCollection);
    }

    public override void Shutdown()
    {
        _modificators.RemoveStatsCollection(_mob.StatsCollection);
    }
}
