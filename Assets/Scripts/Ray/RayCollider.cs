using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Umb16.Extensions;
using System.Linq;

public class RayCollider : MonoBehaviour
{
    private class RayTimer
    {
        public float NextTick;
        public IDamagable Mob;
    }
    private float tickTime = .5f;
    private AsyncReactiveProperty<Player> _player;
    private MiningDamageReceiver _miningDamageReceiver;
    private Dictionary<IDamagable, RayTimer> _mobs = new Dictionary<IDamagable, RayTimer>();
    private LayeredTile[] _oldTilesArray = { };

    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player, MiningDamageReceiver miningDamageReceiver)
    {
        _player = player;
        _miningDamageReceiver = miningDamageReceiver;
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

    private void FixedUpdate()
    {
        var tiles = _miningDamageReceiver.Tiles.GetInRadius(transform.position, transform.localScale.x * .5f);
        foreach (var tile in tiles)
        {
            if (!_oldTilesArray.Contains(tile))
            {
                OnTriggerEnter(tile);
            }
        }
        foreach (var item in _oldTilesArray)
        {
            if (!tiles.Contains(item))
                OnTriggerExit(item);
        }
        _oldTilesArray = tiles;
    }

    private void Update()
    {
        for (int i = 0; i < LiquidTest.Instance.LiquidsList.Count; i++)
        {
            Liquid item = LiquidTest.Instance.LiquidsList[i];
            if ((item.Position - transform.position).SqrMagnetudeXY() < Mathf.Pow(item._colliderRadius + transform.localScale.x * .5f, 2))
            {
                item.ReceiveDamage(new DamageMessage(_player.Value, item, _player.Value.RayDmg, DamageSources.RayInitial));
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
