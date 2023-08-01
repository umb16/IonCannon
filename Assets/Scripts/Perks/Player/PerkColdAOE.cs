using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Umb16.Extensions;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PerkColdAOE : PerkBase
{
    private IMob _mob;
    private float _radius;
    private List<IMob> _mobs = new();
    private DamageController _damageController;

    public PerkColdAOE(float radius)
    {
        _radius = radius;
    }

    public override void Add(IPerk perk)
    {
        throw new System.NotImplementedException();
    }

    public override void Init(IMob mob)
    {
        _mob = mob;
        _mobs = mob.AllMobs;
        _damageController = _mob.DamageController;
        _damageController.Damage += OnDamage;

    }

    private void OnDamage(DamageMessage message)
    {
        if (message.Target == _mob && message.DamageSource != DamageSources.Heal)
        {
            TakingDamage();
        }
        
    }

    public override void Shutdown()
    {
        _damageController.Damage -= OnDamage;
    }

    private void TakingDamage()
    {
        for (int i = 0; i < _mobs.Count; i++)
        {
            var mob = _mobs[i];
            if (mob == _mob)
                continue;
            if ((mob.Position - _mob.Position).SqrMagnetudeXY() < _radius * _radius)
            {
                mob.AddPerk(new PerkFrostbiteEffect());
            }
        }
    }
}
