using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks.Linq;
using Zenject;

public class PerkPIonization : WithId,IPerk
{
    public PerkType Type => PerkType.Ionization;

    private IMob _mob;
    private DamageController _damageController;

    private float _damage;  //_mob.StatsCollection.GetStat(StatType.RayDamage).Value  * .1f;
    private float _duration = 10;

    public bool IsCommon => true;

    public void Add(IPerk perk)
    {

    }

    public PerkPIonization(DamageController damageController, float damage, float duration)
    {
        _damageController = damageController;

        _damageController.Damage += OnDamage;
        _damage = damage;
        _duration = duration;
    }

    public void Init(IMob mob)
    {
        _mob = mob;
    }

    private void OnDamage(DamageMessage message)
    {
        if (message.DamageSource == DamageSources.RayInitial && message.Attacker == _mob && message.Target is IMob target)
        {
            target.AddPerk(new PerkUIonizationEffect(_mob, _damage, _duration));
        }
    }

    public void Shutdown()
    {
        _damageController.Damage -= OnDamage;
    }

    public string GetDescription()
    {
        return null;
    }
}
