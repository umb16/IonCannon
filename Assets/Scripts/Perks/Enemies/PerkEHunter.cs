using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Umb16.Extensions;
using UnityEngine;

public class PerkEHunter : PerkEStandart
{
    private float _huntDistance = 10;
    private IStatModificator _statModificator = new StatModificator(3.5f, StatModificatorType.Additive, StatType.MovementSpeed);
    private bool _inRadius;
    private IDisposable _updateDisposible;

    public override PerkType Type => PerkType.EHunter;

    public override void Init(IMob mob)
    {
        base.Init(mob);
        _updateDisposible = UniTaskAsyncEnumerable.EveryUpdate().Subscribe(Update);
    }

    private void Update(AsyncUnit arg)
    {
        if (_mob.Player!=null && (_mob.Player.Position - _mob.Position).SqrMagnetudeXY() < _huntDistance * _huntDistance)
        {
            if (!_inRadius)
            {
                OnRadiusEnter();
            }
            _inRadius = true;
        }
        else
        {
            if (_inRadius)
            {
                OnRadiusExit();
            }
            _inRadius = false;
        }
    }

    private void OnRadiusExit()
    {
        _mob.StatsCollection.RemoveModificator(_statModificator);
        _mob.SetAnimVariable("Run", false);
    }

    private void OnRadiusEnter()
    {
        _mob.StatsCollection.AddModificator(_statModificator);
        _mob.SetAnimVariable("Run", true);
    }

    public override void Shutdown()
    {
        base.Shutdown();
        _updateDisposible.Dispose();
    }
}
