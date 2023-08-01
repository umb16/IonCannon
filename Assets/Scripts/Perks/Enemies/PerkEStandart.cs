using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PerkEStandart : WithTag, IPerk
{
    public abstract PerkType Type { get; }

    public bool IsCommon => false;

    protected IMob _mob;

    public virtual void Init(IMob mob)
    {
        _mob = mob;
    }

    public virtual void Shutdown()
    {

    }

    public virtual void Add(IPerk perk)
    {

    }

    public string GetDescription()
    {
        throw new System.NotImplementedException();
    }
}
