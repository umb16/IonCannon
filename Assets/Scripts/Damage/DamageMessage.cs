using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageMessage
{
    public Mob Attacker;
    public Mob Target;
    public float Damage;
    public DamageSources DamageSource;

    public DamageMessage(Mob attacker, Mob target, float damage, DamageSources damageSource)
    {
        Attacker = attacker;
        Target = target;
        Damage = damage;
        DamageSource = damageSource;
    }
}
