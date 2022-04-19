using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks.Linq;

public class PerkPIonization : WithId,IPerk
{
    public PerkType Type => PerkType.Ionization;

    private IMob _mob;

    private float Damage => _mob.StatsCollection.GetStat(StatType.RayDamage).Value  * .1f;

    public bool IsCommon => false;

    public void Add(IPerk perk)
    {

    }

    public void Init(IMob mob)
    {
        _mob = mob;
        _mob.DamageController.Damage += OnDamage;
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

    }
}
