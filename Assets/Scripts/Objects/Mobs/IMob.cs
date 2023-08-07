using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IMob : IMovable, IHavePosition, IDamageable
{
    int ID { get; }
    MobType Type { get; }
    Vector3 GroundCenterPosition { get; }
    DamageController DamageController { get; }
    GameData GameData { get; }
    ComplexStat HP { get; }
    bool IsReady { get; }
    Player Player { get; }
    List<IMob> AllMobs { get; }
    MobSpawner Spawner { get; }
    ComplexStat MovementSpeed { get; }
    StandardStatsCollection StatsCollection { get; }
    Inventory Inventory { get; }
    bool IsDead { get; }
    event Action PickedUpScoreGem;

    void AddPerk(IPerk perk);
    void RemovePerksByType(PerkType perkType);
    bool ContainPerk(PerkType perkType);
    void Die(DamageMessage message);
    UniTask AddFx(Fx fx);
    void RemoveFx(Fx fx);

    void AddForce(Vector2 force, ForceMode mode);
    void SetAnimVariable(string name, bool value);
    void RemovePerk(IPerk perk);
    void Destroy();
    void SetInvulnerability(bool value);
    
}