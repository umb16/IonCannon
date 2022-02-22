using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageMessage
{
    public Mob Attacker;
    public Mob Target;
    public float Damage;
    public DamageSources DamageSource;
}
