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

    private float Damage => _mob.StatsCollection.GetStat(StatType.RayDamage).Value  * .1f;

    public bool IsCommon => false;

    public void Add(IPerk perk)
    {

    }

    public PerkPIonization(DamageController damageController)
    {
        _damageController = damageController;

        _damageController.Damage += OnDamage;
    }

    public void Init(IMob mob)
    {
        _mob = mob;
    }

    private void OnDamage(DamageMessage message)
    {
        if (message.DamageSource == DamageSources.Ray && message.Attacker == _mob)
        {
            message.Target.AddPerk(new PerkUIonizationEffect(_mob, Damage));
        }
    }

    public void Shutdown()
    {
        _damageController.Damage -= OnDamage;
    }
}
