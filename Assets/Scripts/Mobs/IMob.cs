using System;
using UnityEngine;

public interface IMob
{
    DamageController DamageController { get; }
    GameData GameData { get; }
    ComplexStat HP { get; }
    bool IsReady { get; }
    Player Player { get; }
    ComplexStat MovementSpeed { get; }
    StandartStatsCollection StatsCollection { get; }

    void AddPerk(Func<Mob, IPerk> perkGenerator, int level = 0);
    public void ReceiveDamage(DamageMessage message);
    void Die(DamageMessage message);
    void MoveTo(Vector3 target);
    void Stop();
}