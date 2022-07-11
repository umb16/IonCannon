using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Umb16.Extensions;

public class RayCollider : MonoBehaviour
{
    private class RayTimer
    {
        public float NextTick;
        public IDamagable Mob;
    }

    private float tickTime = .5f;
    private AsyncReactiveProperty<Player> _player;
    private Dictionary<IDamagable, RayTimer> _mobs = new Dictionary<IDamagable, RayTimer>();

    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player)
    {
        _player = player;
    }

    private void OnTriggerEnter(Collider col)
    {
        var mob = col.gameObject.GetComponent<IDamagable>();
        if (mob != null)
        {
            OnTriggerEnter(mob);
        }
    }

    private void OnTriggerEnter(IDamagable mob)
    {
        _mobs.Add(mob, new RayTimer() { NextTick = Time.time + tickTime, Mob = mob });
        mob.ReceiveDamage(new DamageMessage(_player.Value, mob, _player.Value.RayDmg, DamageSources.RayInitial, .5f));
    }

    private void OnTriggerExit(Collider col)
    {

        var mob = col.gameObject.GetComponent<IDamagable>();
        if (mob != null)
        {
            OnTriggerExit(mob);
        }
    }

    private void OnTriggerExit(IDamagable mob)
    {
        _mobs.Remove(mob);
    }

    private void Update()
    {
        for (int i = 0; i < LiquidTest.Instance.LiquidsList.Count; i++)
        {
            Liquid item = LiquidTest.Instance.LiquidsList[i];
            if ((item.Position - transform.position).SqrMagnetudeXY() < item._colliderRadius * item._colliderRadius)
            {
                item.ReceiveDamage(new DamageMessage(_player.Value, item, _player.Value.RayDmg, DamageSources.RayInitial, .5f));
                i--;
            }
        }
        foreach (var mob in _mobs)
        {
            if (mob.Value.NextTick < Time.time)
            {
                mob.Value.NextTick = Time.time + tickTime;
                mob.Value.Mob.ReceiveDamage(new DamageMessage(_player.Value, mob.Value.Mob, _player.Value.RayDmg * .5f, DamageSources.Ray, .5f));
            }
        }
    }
}
