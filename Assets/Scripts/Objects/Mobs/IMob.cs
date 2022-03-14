using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IMob
{
    int ID { get; }
    Vector3 Position { get; }
    DamageController DamageController { get; }
    GameData GameData { get; }
    ComplexStat HP { get; }
    bool IsReady { get; }
    Player Player { get; }
    HashSet<IMob> AllMobs { get; }
    ComplexStat MovementSpeed { get; }
    StandartStatsCollection StatsCollection { get; }
    bool IsDead { get; }

    void SetPosition(float x, float y);
    void SetPosition(Vector3 vector);

    void AddPerk(Func<IMob, IPerk> perkGenerator, int level = 0);
    void RemovePerk(PerkType perkType);
    bool ContainPerk(PerkType perkType);
    public void ReceiveDamage(DamageMessage message);
    void Die(DamageMessage message);
    void MoveTo(Vector3 target);
    void OnDie();
    UniTask AddFx(Fx fx);
    void RemoveFx(Fx fx);
}