using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RayCollider : MonoBehaviour
{
    private class RayTimer
    {
        public float NextTick;
        public IMob Mob;
    }

    private float tickTime = .5f;
    private Player _player;
    private List<RayTimer> _mobs = new List<RayTimer>();

    [Inject]
    private void Construct(Player player)
    {
        _player = player;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var mob = col.gameObject.GetComponent<IMob>();
        if (mob != null)
        {
            mob.ReceiveDamage(new DamageMessage(_player, mob, _player.RayDmg, DamageSources.Ray));
            _mobs.Add(new RayTimer() { NextTick = Time.time + tickTime, Mob = mob });
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        var mob = col.gameObject.GetComponent<IMob>();
        if (mob != null)
        {
            for (int i = 0; i < _mobs.Count; i++)
            {
                if (_mobs[i].Mob == mob)
                {
                    _mobs.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < _mobs.Count; i++)
        {
            var mob = _mobs[i];
            if (mob.NextTick < Time.time)
            {
                mob.NextTick = Time.time + tickTime;
                mob.Mob.ReceiveDamage(new DamageMessage(_player, mob.Mob, _player.RayDmg * .5f, DamageSources.Ray));
            }
        }
    }
}
