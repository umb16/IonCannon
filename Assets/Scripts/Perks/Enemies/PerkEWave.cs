using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkEWave : PerkEStandart
{
    public override PerkType Type => PerkType.EWaveFactor;

    public int Wave => _mob.GameData.Wave;

    private IStatModificator[] _modificators;

    public override async void Init(IMob mob)
    {
        await UniTask.WaitUntil(() => mob.IsReady);
        base.Init(mob);
        _modificators = new[]
        {
            new StatModificator((x) => x * (Wave + 1), StatModificatorType.TransformChain, StatType.Score),
            new StatModificator((x) => x * (Wave*.7f + 1), StatModificatorType.TransformChain, StatType.MaxHP),
            new StatModificator((x) => x * (Wave*.1f + 1), StatModificatorType.TransformChain, StatType.Damage),
        };

        _mob.StatsCollection.AddModificators(_modificators);
        mob.HP.SetBaseValue(mob.StatsCollection.GetStat(StatType.MaxHP).Value);
    }

    public override void Shutdown()
    {
        _mob.StatsCollection.RemoveModificators(_modificators);
    }
}
