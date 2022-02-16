using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatsCollectionsDB
{
    public static PlayerStats StandartPlayer()
    {
        ComplexStat maxHP = new ComplexStat(3);
        ComplexStat HP = new ComplexStat(maxHP.Value);
        maxHP.ValueChanged+= (x) => HP.SetBaseValue(Mathf.Min(HP.BaseValue, maxHP.Value));
        return new PlayerStats(new (StatType type, ComplexStat stat)[]
         {
            (StatType.Speed, new ComplexStat(3)),
            (StatType.RaySpeed, new ComplexStat(6)),
            (StatType.RayPathLenght, new ComplexStat(10)),
            (StatType.RayDelay, new ComplexStat(1.3f)),
            (StatType.RayDamageAreaRadius, new ComplexStat(1)),
            (StatType.RayDamage, new ComplexStat(4)),
            (StatType.MaxHP, maxHP),
            (StatType.HP, HP),
         });
    }
}
