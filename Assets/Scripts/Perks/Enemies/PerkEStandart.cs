using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkEStandart : IPerk
{
    public virtual PerkType Type { get; }

    public string Name => "Its enemy perk";

    public string Description => "Its enemy perk";

    public int Level => 1;

    public bool Maxed => true;

    public int MaxLevel => 1;

    public void AddLevel()
    {
        Debug.Log("Its enemy perk");
    }

    public void SetLevel(int level)
    {
        Debug.Log("Its enemy perk");
    }

    public virtual void Shutdown()
    {
        throw new System.NotImplementedException();
    }
}
