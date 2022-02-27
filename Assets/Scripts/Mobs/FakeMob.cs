using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeMob : IMob
{
    public DamageController DamageController { get; private set; }

    public GameData GameData { get; private set; }

    public ComplexStat HP { get; private set; }

    public bool IsReady { get; private set; }

    public Player Player { get; private set; }

    public ComplexStat MovementSpeed { get; private set; }

    public StandartStatsCollection StatsCollection { get; private set; }

    public FakeMob()
    {
        StatsCollection = StatsCollectionsDB.StandartPlayer();
    }

    public void AddPerk(Func<Mob, IPerk> perkGenerator, int level = 0)
    {
        throw new NotImplementedException();
    }
    public void Die(DamageMessage message)
    {
        throw new NotImplementedException();
    }
    public void MoveTo(Vector3 target)
    {
        throw new NotImplementedException();
    }
    public void Stop()
    {
        throw new NotImplementedException();
    }
}
