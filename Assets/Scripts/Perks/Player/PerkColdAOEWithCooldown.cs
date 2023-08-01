using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Umb16.Extensions;
using Random = UnityEngine.Random;

public class PerkColdAOEWithCooldown : PerkCommonBase
{
    private IMob _mob;
    private List<IDisposable> _loops = new();
    private int _maxAffectedBy;
    private float _radius;
    private float _cooldown;
    private List<IMob> _mobs = new();
    private CooldownIndicator _indicator;
    private UICooldownsManager _cooldownsPanel;
    private float _lastSpawnTime;
    private float NextSpawnTime => _lastSpawnTime + _cooldown;

    public PerkColdAOEWithCooldown(UICooldownsManager cooldownsManager,int maxAffectedBy, float radius, float cooldown)
    {
        _maxAffectedBy = maxAffectedBy;
        _radius = radius;
        _cooldown = cooldown;
        _cooldownsPanel = cooldownsManager;
        SetType(PerkType.ColdAOE);
    }

    public override void Init(IMob mob)
    {
        _lastSpawnTime = Time.time;
        _mob = mob;
        _mobs = mob.AllMobs;
        _loops.Add(UniTaskAsyncEnumerable.Timer(TimeSpan.FromSeconds(_cooldown), TimeSpan.FromSeconds(_cooldown)).Subscribe(Update));

        _cooldownsPanel.AddIndiacator(Addresses.Ico_PoleOfCold).ContinueWith(x =>
        {
            _indicator = x;
            _loops.Add(UniTaskAsyncEnumerable.EveryUpdate().Subscribe(_ =>
            {
                _indicator.SetTime(NextSpawnTime - Time.time, _cooldown);
            }
            ));
        }).Forget();
    }

    public override void Shutdown()
    {
        foreach (var loop in _loops)
        {
            loop.Dispose();
        }
        _indicator?.Destroy();
    }

    private void Update(AsyncUnit obj)
    {
        _lastSpawnTime = Time.time;
        List<IMob> mobsAffectedBy = new();

        for (int i = 0; i < _mobs.Count; i++)
        {
            var mob = _mobs[i];
            if (mob == _mob)
                continue;
            if ((mob.Position - _mob.Position).SqrMagnetudeXY() < _radius * _radius)
            {
                mobsAffectedBy.Add(mob);
            }
        }

        for (int i = 0; i < _maxAffectedBy; i++)
        {
            if (mobsAffectedBy.Count != 0)
            {
                var randomMob = mobsAffectedBy[Random.Range(0, mobsAffectedBy.Count)];
                randomMob.AddPerk(new PerkFrostbiteEffect());
                mobsAffectedBy.Remove(randomMob);
            }          
        }
    }
}
