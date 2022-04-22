using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatsCollectionsDB
{
    public static StandartStatsCollection StandartPlayer()
    {
        ComplexStat maxHP = new ComplexStat(3);
        ComplexStat HP = new ComplexStat(maxHP.Value, (x) => Mathf.Min(x, maxHP.Value));
        maxHP.ValueChanged += (x) => HP.SetBaseValue(Mathf.Min(HP.BaseValue, maxHP.Value));
        return new StandartStatsCollection(new (StatType type, ComplexStat stat)[]
         {
            (StatType.MovementSpeed, new ComplexStat(6)),
            (StatType.RaySpeed, new ComplexStat(6)),
            (StatType.RayPathLenght, new ComplexStat(10)),
            (StatType.RayDelay, new ComplexStat(1.3f, (x)=>Mathf.Max(0,x))),
            (StatType.RayDamageAreaRadius, new ComplexStat(1)),
            (StatType.RayDamage, new ComplexStat(4)),
            (StatType.MaxHP, maxHP),
            (StatType.HP, HP),
            (StatType.Size, new ComplexStat(1)),
            (StatType.LifeSupport, new ComplexStat(1).SetBaseLimit(0, 1)),
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
         });
    }
}
