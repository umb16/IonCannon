using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPerk
{
    public string Name { get; }
    public string Description { get; }
    public int Level { get;}
    public bool Maxed { get; }
    public int MaxLevel { get; }
    public void SetStatsCollection(IStatsCollection collection);
    public void SetLevel(int level);
    public void AddLevel();
    public void OnUpdate();
    public void OnEnemyHit(DamageSources source, IStatsCollection collection);

}
