using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks.Linq;
using Zenject;

public class PerkPIonization : PerkCommonBase
{
    private IMob _mob;
    private DamageController _damageController;

    private float _damage;
    private float _duration = 10;

    public PerkPIonization(DamageController damageController, float damage, float duration)
    {
        _damageController = damageController;
        _damage = damage;
        _duration = duration;
        SetType(PerkType.Ionization);
    }

    public override void Init(IMob mob)
    {
        _mob = mob;
        _damageController.Damage += OnDamage;
    }

    private void OnDamage(DamageMessage message)
    {
        if (message.DamageSource == DamageSources.RayInitial && message.Attacker == _mob && message.Target is IMob target)
        {
            target.AddPerk(new PerkUIonizationEffect(_mob, _damage, _duration));
        }
    }

    public override void Shutdown()
    {
        _damageController.Damage -= OnDamage;
    }
}
