using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Umb16.Extensions;
using UnityEngine;

public class PerkESpeedAura : PerkEStandart
{
    public override PerkType Type => PerkType.ESpeedAura;

    private HashSet<IMob> _mobsInRadius = new HashSet<IMob>();


    private float _distanceValue = 12;

    private Fx _aura = new Fx("Fx_Aura0", FxPosition.Ground);
    private IDisposable _disposable;

    private bool _enabled;


    public override void Init(IMob mob)
    {
        base.Init(mob);
        mob.AddFx(_aura);
        _enabled = true;
        _disposable = UniTaskAsyncEnumerable.EveryUpdate().Subscribe(Update);
    }

    private void Update(AsyncUnit obj)
    {
        if (!_enabled)
            return;
        foreach (var mob in _mob.AllMobs)
        {
            if (mob == _mob || mob.Type == MobType.Object)
                continue;
            if ((_mob.Position - mob.Position).SqrMagnitudeXY() < _distanceValue * _distanceValue)
            {
                if (!_mobsInRadius.Contains(mob) && !mob.ContainPerk(PerkType.ESpeedAuraEffect))
                {
                    OnEnter(mob);
                }
            }
            else
            {
                if (_mobsInRadius.Contains(mob))
                {
                    OnExit(mob);
                }
            }
        }
    }

    private void RemoveAll()
    {
        foreach (var mob in _mobsInRadius)
        {
            if (mob != null && !mob.IsDead)
                mob.RemovePerksByType(PerkType.ESpeedAuraEffect);
        }
        _mob.RemoveFx(_aura);
    }

    private void OnExit(IMob mob)
    {
        _mobsInRadius.Remove(mob);
        mob.RemovePerksByType(PerkType.ESpeedAuraEffect);
    }

    private void OnEnter(IMob mob)
    {
        _mobsInRadius.Add(mob);
        mob.AddPerk(new PerkESpeedAuraEffect());
    }

    public override void Shutdown()
    {
        _disposable.Dispose();
        _enabled = false;
        RemoveAll();
    }
}
