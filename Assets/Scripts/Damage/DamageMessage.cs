using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageMessage
{
    public IMob Attacker;
    public IMob Target;
    public float Damage;
    public DamageSources DamageSource;

    public DamageMessage(IMob attacker, IMob target, float damage, DamageSources damageSource)
    {
        Attacker = attacker;
        Target = target;
        Damage = damage;
        DamageSource = damageSource;
    }
}
