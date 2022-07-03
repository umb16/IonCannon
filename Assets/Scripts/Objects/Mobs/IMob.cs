using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IMob
{
    int ID { get; }
    MobType Type { get; }
    Vector3 Position { get; }
    Vector3 GroundCenterPosition { get; }
    DamageController DamageController { get; }
    GameData GameData { get; }
    ComplexStat HP { get; }
    bool IsReady { get; }
    Player Player { get; }
    List<IMob> AllMobs { get; }
    MobSpawner Spawner { get; }
    ComplexStat MovementSpeed { get; }
    StandartStatsCollection StatsCollection { get; }
    Inventory Inventory { get; }
    bool IsDead { get; }

    void SetPosition(float x, float y);
    void SetPosition(Vector3 vector);
    void AddPerk(IPerk perk);
    void RemovePerksByType(PerkType perkType);
    bool ContainPerk(PerkType perkType);
    public void ReceiveDamage(DamageMessage message);
    void Die(DamageMessage message);
    void MoveTo(Vector2 target);
    UniTask AddFx(Fx fx);
    void RemoveFx(Fx fx);

    void AddForce(Vector2 force, ForceMode mode);
    void SetAnimVariable(string name, bool value);
    void RemovePerk(IPerk perk);

    void Destroy();
}