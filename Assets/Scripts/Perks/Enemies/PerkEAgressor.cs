using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Umb16.Extensions;
using UnityEngine;

public class PerkEAgressor : PerkEStandart
{
    private IStatModificator _statModificator = new StatModificator(0.7f, StatModificatorType.Additive, StatType.MovementSpeed);
    private IDisposable _updateDisposible;
    private bool _once;

    public override PerkType Type => PerkType.EAgressor;

    public override void Init(IMob mob)
    {
        base.Init(mob);
        _updateDisposible = UniTaskAsyncEnumerable.EveryUpdate().Subscribe(Update);
    }

    private void Update(AsyncUnit arg)
    {
        if (!_once && _mob.HP.Value < _mob.StatsCollection.GetStat(StatType.MaxHP).Value)
        {
            _once = true;
            _mob.StatsCollection.AddModificator(_statModificator);
        }
    }

    public override void Shutdown()
    {
        base.Shutdown();
        _updateDisposible.Dispose();
    }
}
