using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class PerkPShiftSystem : WithId, IPerk
{
    private IMob _mob;

    public PerkType Type => PerkType.PShift;

    public bool IsCommon => false;
    private Timer _timer;
    private StatModificator _modificator;
    private StatModificator _modificatorSpeedUp = new StatModificator(.5f, StatModificatorType.Multiplicative, StatType.MovementSpeed);

    public PerkPShiftSystem()
    {

    }
    public void Init(IMob mob)
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

    public void Shutdown()
    {
        _timer?.ForceEnd();
        _mob.DamageController.Damage -= OnDamage;
        _mob.StatsCollection.RemoveModificator(_modificator);
    }

    public void Add(IPerk perk)
    {

    }

    public string GetDescription()
    {
        return null;
    }
}
