using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks.Linq;

public class PerkPIonization : WithId,IPerk
{
    public PerkType Type => PerkType.Ionization;

    public string Name => LocalizationManager.Instance.GetPhrase(LocKeys.Ionization);

    public string Description => LocalizationManager.Instance.GetPhrase(LocKeys.Ionization);

    public int Level { get; private set; }

    public bool Maxed => Level == MaxLevel;

    public int MaxLevel => 3;

    private IMob _mob;

    private float Damage => _mob.StatsCollection.GetStat(StatType.RayDamage).Value * Level * .1f;

    public bool IsÑommon => false;

    public void Add(IPerk perk)
    {

    }

    public void AddLevel()
    {
        Level++;
    }

    public void Init(IMob mob)
    {
        _mob = mob;
        _mob.DamageController.Damage += OnDamage;
    }

    private void OnDamage(DamageMessage message)
    {
        if (Level > 0 && message.DamageSource == DamageSources.Ray && message.Attacker == _mob)
        {
            message.Target.AddPerk(new PerkUIonizationEffect(_mob, Damage));
        }
    }

    public void SetLevel(int level)
    {
        Level = level;
    }

    public void Shutdown()
    {

    }
}
