using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Umb16.Extensions;
using UnityEngine;

public class PerkAura : WithId, IPerk
{
    public PerkType Type { get; private set; }
    private PerkType _effectType;
    private Fx _perkFx;
    private Fx _effectFx;
    private bool _tagetPlayer;

    public bool IsCommon => false;

    private HashSet<IMob> _mobsInRadius = new HashSet<IMob>();

    private float _distanceValue = 3;

    // private Fx _aura = new Fx("Fx_Aura0", FxPosition.Ground);
    private IDisposable _disposable;
    private IMob _mob;
    private bool _enabled;
    private IStatModificator[] _modificators;

    public PerkAura(PerkType perkType, PerkType effectType, IStatModificator[] modificators,
        float radius, bool tagetPlayer = false, Fx perkFx = null, Fx effectFx = null)
    {
        Type = perkType;
        _effectType = effectType;
        _perkFx = perkFx;
        _effectFx = effectFx;
        _tagetPlayer = tagetPlayer;
        _modificators = modificators;
        _distanceValue = radius;
    }

    public void Init(IMob mob)
    {
        _mob = mob;
        if (_perkFx != null)
            mob.AddFx(_perkFx);
        _enabled = true;
        _disposable = UniTaskAsyncEnumerable.EveryUpdate().Subscribe(Update);
    }

    private void Update(AsyncUnit obj)
    {
        if (!_enabled)
            return;

        if (_tagetPlayer)
        {
            CheckMob(_mob.Player);
        }
        else
        {
            foreach (var mob in _mob.AllMobs)
            {
                CheckMob(mob);
            }
        }

    }

    private void CheckMob(IMob mob)
    {
        if (mob == null)
            return;
        if (mob == _mob || mob.Type == MobType.Object)
            return;
        if ((_mob.Position - mob.Position).SqrMagnetudeXY() < _distanceValue * _distanceValue)
        {
            if (!_mobsInRadius.Contains(mob) && !mob.ContainPerk(_effectType))
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

    private void RemoveAll()
    {
        foreach (var mob in _mobsInRadius)
        {
            if (mob != null && !mob.IsDead)
                mob.RemovePerksByType(_effectType);
        }
        if (_perkFx != null)
            _mob.RemoveFx(_perkFx);
    }

    private void OnExit(IMob mob)
    {
        _mobsInRadius.Remove(mob);
        mob.RemovePerksByType(_effectType);
    }

    private void OnEnter(IMob mob)
    {
        _mobsInRadius.Add(mob);
        mob.AddPerk(new PerkAuraEffect(_effectType, _effectFx, _modificators));
    }

    public void Shutdown()
    {
        _disposable.Dispose();
        _enabled = false;
        RemoveAll();
    }

    public void Add(IPerk perk)
    {
        //throw new NotImplementedException();
    }
}
