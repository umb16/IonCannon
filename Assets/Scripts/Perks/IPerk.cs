using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IPerk
{
    public string Name { get; }
    public string Description { get; }
    public int Level { get;}
    public bool Maxed { get; }
    public int MaxLevel { get; }
    public void SetParent(Mob mob);
    public void SetLevel(int level);
    public void AddLevel();
}
