using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour, IMovable
{
    private List<IPerk> _perks = new List<IPerk>();
    public StandartStatsCollection StatsCollection { get; private set; }


    public void AddPerk(Func<IStatsCollection, IPerk> perkGenerator)
    {
        _perks.Add(perkGenerator(StatsCollection));
    }

    public void ReceiveDamage(DamageSources source, float value)
    {
        StatsCollection.GetStat(StatType.HP).AddBaseValue(-value);
        foreach (var perk in _perks)
        {
            perk.OnReceiveDamage(source);
        }
    }

    public void MoveTo(Vector3 target)
    {

    }

    private void Update()
    {
        foreach (var perk in _perks)
        {
            perk.OnUpdate();
        }
    }
}
