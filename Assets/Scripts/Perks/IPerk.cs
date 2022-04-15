using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IPerk
{
    int InstanceId { get; }
    bool Is—ommon { get; }
    PerkType Type { get; }
    string Name { get; }
    string Description { get; }
    int Level { get;}
    bool Maxed { get; }
    int MaxLevel { get; }
    void SetLevel(int level);
    void AddLevel();
    void Shutdown();
    void Init(IMob mob);
    void Add(IPerk perk);
}
