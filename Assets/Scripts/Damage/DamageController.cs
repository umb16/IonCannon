using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks.Linq;

public class DamageController
{
    public event Action<DamageMessage> Damage;
    public event Action<DamageMessage> Die;

    public void SendDamage(DamageMessage message)
    {
        Damage?.Invoke(message);
    }
    public void SendDie(DamageMessage message)
    {
        Die?.Invoke(message);
    }
}
