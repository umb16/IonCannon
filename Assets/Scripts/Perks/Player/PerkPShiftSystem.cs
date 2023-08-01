using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PerkPShiftSystem : PerkBase
{
    private IMob _mob;

    private Timer _timer;
    private StatModificator _modificator;
    private StatModificator _modificatorSpeedUp = new StatModificator(.5f, StatModificatorType.Multiplicative, StatType.MovementSpeed);

    public PerkPShiftSystem()
    {
        SetType(PerkType.PShift);
    }
    public override void Init(IMob mob)
    {
        _mob = mob;
        _mob.DamageController.Damage += OnDamage;
        _modificator = new StatModificator(-1, StatModificatorType.Additive, StatType.Defence);
        _mob.StatsCollection.AddModificator(_modificator);
    }

    private void OnDamage(DamageMessage msg)
    {
        if (msg.Target != _mob)
            return;
        if (msg.Damage <= 0)
            return;
        if (msg.DamageSource == DamageSources.Heal)
            return;
        _mob.SetInvulnerability(true);
        _mob.StatsCollection.AddModificator(_modificatorSpeedUp);
        _timer?.Stop();
        _timer = new Timer(2).SetEnd(() =>
        { 
            _mob.SetInvulnerability(false);
            _mob.StatsCollection.RemoveModificator(_modificatorSpeedUp);
        });
    }

    public override void Shutdown()
    {
        _timer?.ForceEnd();
        _mob.DamageController.Damage -= OnDamage;
        _mob.StatsCollection.RemoveModificator(_modificator);
    }

    public override void Add(IPerk perk)
    {

    }
}
