using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatsCollectionsDB
{
    public static StandartStatsCollection StandartPlayer()
    {
        int hp = 10;
        ComplexStat maxHP = new ComplexStat(hp);
        ComplexStat HP = new ComplexStat(maxHP.Value, (x) => Mathf.Min(x, maxHP.Value));
        maxHP.ValueChanged += (x) => HP.SetBaseValue(Mathf.Min(HP.BaseValue, maxHP.Value));
        return new StandartStatsCollection(new (StatType type, ComplexStat stat)[]
         {
            (StatType.MovementSpeed, new ComplexStat(6)),
            (StatType.RaySpeed, new ComplexStat(6)),
            (StatType.RayPathLenght, new ComplexStat(10)),
            (StatType.RayDelay, new ComplexStat(1.3f, (x)=>Mathf.Max(0,x))),
            (StatType.RayDamageAreaRadius, new ComplexStat(1)),
            (StatType.RayDamage, new ComplexStat(40, (x)=>Mathf.Max(1,x))),
            (StatType.MaxHP, maxHP),
            (StatType.HP, HP),
            (StatType.Size, new ComplexStat(1)),
            (StatType.LifeSupport, new ComplexStat(1).SetBaseLimit(0, 1)),
            (StatType.PickupRadius, new ComplexStat(2)),
            (StatType.RayError, new ComplexStat(0, (x)=>Mathf.Min(10,x))),
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
