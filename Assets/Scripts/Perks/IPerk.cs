using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IPerk
{
    int InstanceId { get; }
    bool IsCommon { get; }
    PerkType Type { get; }
    void Shutdown();
    void Init(IMob mob);
    void Add(IPerk perk);

    string GetDescription();
}
