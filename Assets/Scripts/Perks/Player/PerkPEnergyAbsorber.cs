using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks.Linq;
using Zenject;

public class PerkPEnergyAbsorber : PerkCommonBase
{
    private IMob _mob;
    private DamageController _damageController;
    private float _amount = 5;

    public PerkPEnergyAbsorber(DamageController damageController, float amount)
    {
        _damageController = damageController;
        _amount = amount;
        SetType(PerkType.EnergyAbsorber);
    }

    private void OnDie(DamageMessage obj)
    {
        if (obj.DamageSource != DamageSources.RayInitial || obj.DamageSource != DamageSources.Ray)
        {
            if (_mob is Player player)
            {
                player.AddEnergy(_amount);
            }
        }
    }

    public override void Init(IMob mob)
    {
        _mob = mob;
        _damageController.Die += OnDie;
    }

    public override void Shutdown()
    {
        _damageController.Die -= OnDie;
    }
}
