using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IPerk
{
    PerkType Type { get; }
    string Name { get; }
    string Description { get; }
    int Level { get;}
    bool Maxed { get; }
    int MaxLevel { get; }
    void SetParent(IMob mob);
    void SetLevel(int level);
    void AddLevel();
    void Shutdown();
}
