using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageMessage
{
    public IMob Attacker;
    public IDamagable Target;
    public float Damage;
    public float StunTime;
    public DamageSources DamageSource;

    public DamageMessage(IMob attacker, IDamagable target, float damage, DamageSources damageSource, float stun = 0)
    {
        Attacker = attacker;
        Target = target;
        Damage = damage;
        DamageSource = damageSource;
        StunTime = stun;
    }
}
