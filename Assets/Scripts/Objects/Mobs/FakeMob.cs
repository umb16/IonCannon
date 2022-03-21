using Cysharp.Threading.Tasks;
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

    public int ID => throw new NotImplementedException();

    public Vector3 Position => throw new NotImplementedException();

    public List<IMob> AllMobs => throw new NotImplementedException();

    public bool IsDead => throw new NotImplementedException();

    public Vector3 GroundCenterPosition => throw new NotImplementedException();

    public MobType Type => throw new NotImplementedException();

    public FakeMob()
    {
        StatsCollection = StatsCollectionsDB.StandartPlayer();
    }

    public void AddPerk(Func<IMob, IPerk> perkGenerator, int level = 0)
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
    public void OnDie()
    {
        throw new NotImplementedException();
    }

    public void ReceiveDamage(DamageMessage message)
    {
        throw new NotImplementedException();
    }

    public void SetPosition(float x, float y)
    {
        throw new NotImplementedException();
    }

    public void SetPosition(Vector3 vector)
    {
        throw new NotImplementedException();
    }

    UniTask IMob.AddFx(Fx fx)
    {
        throw new NotImplementedException();
    }

    public void RemoveFx(Fx fx)
    {
        throw new NotImplementedException();
    }

    public void RemovePerk(PerkType perkType)
    {
        throw new NotImplementedException();
    }

    public bool ContainPerk(PerkType perkType)
    {
        throw new NotImplementedException();
    }

    public void AddForce(Vector2 force, ForceMode2D mode)
    {
        throw new NotImplementedException();
    }
}
