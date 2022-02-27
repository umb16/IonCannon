using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IPerk
{
    public PerkType Type { get; }
    public string Name { get; }
    public string Description { get; }
    public int Level { get;}
    public bool Maxed { get; }
    public int MaxLevel { get; }
    public void SetParent(IMob mob);
    public void SetLevel(int level);
    public void AddLevel();
}
