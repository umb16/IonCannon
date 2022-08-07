using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatsCollectionsDB
{
    public static StandartStatsCollection StandartPlayer()
    {
        int hp = 20;
        ComplexStat maxHP = new ComplexStat(hp);
        ComplexStat HP = new ComplexStat(maxHP.Value).SetBaseLimit(0.0f, maxHP.Value);
        maxHP.ValueChanged += (x) =>
        {
            HP.SetBaseLimit(0.0f, maxHP.Value);
            HP.SetBaseValue(Mathf.Min(HP.BaseValue, maxHP.Value));
        };
        ComplexStat capacity = new ComplexStat(100);
        ComplexStat energy = new ComplexStat(capacity.Value).SetBaseLimit(0,capacity.Value);
        capacity.ValueChanged += x =>
        {
            energy.SetBaseLimit(0.0f, capacity.Value);
            energy.SetBaseValue(Mathf.Min(energy.BaseValue, capacity.Value));
        };
        return new StandartStatsCollection(new (StatType type, ComplexStat stat)[]
         {
            (StatType.MovementSpeed, new ComplexStat(6)),
            (StatType.RaySpeed, new ComplexStat(6)),
            (StatType.Energy, energy),
            (StatType.Capacity, capacity),
            (StatType.EnergyRegen, new ComplexStat(4)),
            (StatType.RayDelay, new ComplexStat(1.3f, (x)=>Mathf.Max(0,x))),
            (StatType.RayDamageAreaRadius, new ComplexStat(1)),
            (StatType.RayDamage, new ComplexStat(40, (x)=>Mathf.Max(1,x))),
            (StatType.MaxHP, maxHP),
            (StatType.HP, HP),
            (StatType.Size, new ComplexStat(1)),
            (StatType.LifeSupport, new ComplexStat(1).SetBaseLimit(0, 1)),
            (StatType.PickupRadius, new ComplexStat(3)),
            (StatType.RayError, new ComplexStat(0, (x)=>Mathf.Min(10,x))),
            (StatType.RayReverse, new ComplexStat(0)),
         });
    }
    public static StandartStatsCollection StandartEnemy()
    {
        ComplexStat maxHP = new ComplexStat(3);
        ComplexStat HP = new ComplexStat(maxHP.Value, (x) => Mathf.Min(x, maxHP.Value));
        maxHP.ValueChanged += (x) => HP.SetBaseValue(Mathf.Min(HP.BaseValue, maxHP.Value));
        return new StandartStatsCollection(new (StatType type, ComplexStat stat)[]
         {
            (StatType.MovementSpeed, new ComplexStat(3)),
            (StatType.MaxHP, maxHP),
            (StatType.HP, HP),
            (StatType.Score, new ComplexStat(1)),
            (StatType.Size, new ComplexStat(1)),
            (StatType.Damage, new ComplexStat(0)),
            (StatType.Defence, new ComplexStat(0)),
         });
    }
}
