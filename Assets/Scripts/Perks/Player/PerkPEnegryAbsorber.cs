using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks.Linq;
using Zenject;

public class PerkPEnegryAbsorber : WithId,IPerk
{
    public PerkType Type => PerkType.EnergyAbsorber;

    private IMob _mob;
    private DamageController _damageController;
    private float _amount = 5;

    public bool IsCommon => true;

    public void Add(IPerk perk)
    {

    }

    public PerkPEnegryAbsorber(DamageController damageController, float amount)
    {
        _damageController = damageController;
        _amount = amount;
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

    public void Init(IMob mob)
    {
        _mob = mob;
        _damageController.Die += OnDie;
    }

    public void Shutdown()
    {
        _damageController.Die -= OnDie;
    }

    public string GetDescription()
    {
        return null;
    }
}
