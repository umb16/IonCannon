using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PerkEStandart : WithId, IPerk
{
    public abstract PerkType Type { get; }

    public string Name => "Its enemy perk";

    public string Description => "Its enemy perk";

    public int Level => 1;

    public bool Maxed => true;

    public int MaxLevel => 1;

    public bool IsÑommon => false;

    protected IMob _mob;

    public void AddLevel()
    {
        Debug.Log("Its enemy perk");
    }

    public virtual void Init(IMob mob)
    {
        _mob = mob;
    }

    public void SetLevel(int level)
    {
        Debug.Log("Its enemy perk");
    }

    public virtual void Shutdown()
    {

    }

    public virtual void Add(IPerk perk)
    {

    }
}
