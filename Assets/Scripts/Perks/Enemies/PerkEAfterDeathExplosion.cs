using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Umb16.Extensions;
using UnityEngine;

public class PerkEAfterDeathExplosion : PerkEStandart
{
    private IMob _mob;
    private float _radius = 10;
    private DamageSources _source = DamageSources.Explosion;
    public override PerkType Type => PerkType.EAfterDeathExplosion;
    private Fx _explosion = new Fx("Fx_Explosion", FxPosition.Ground);
    public PerkEAfterDeathExplosion(IMob mob)
    {
        _mob = mob;
        _mob.DamageController.Die += OnDie;
    }

    private void OnDie(DamageMessage msg)
    {
        if (msg.Target == _mob)
        {
            ((Mob)_mob).GetComponent<Animator>().speed = 3;
            new Timer(.49f)
            .SetEnd(() =>
            {
                _mob.DamageController.Die -= OnDie;
                List<IMob> mobs = new List<IMob>(_mob.AllMobs);
                mobs.Add(_mob.Player);
                for (int i = 0; i < mobs.Count; i++)
                {
                    var mob = mobs[i];
                    if ((mob.Position - _mob.Position).SqrMagnetudeXY() < _radius * _radius)
                    {
                        mob.AddForce((1 - (mob.Position - _mob.Position).MagnetudeXY() / _radius) * (mob.Position - _mob.Position).NormalizedXY() * 5, ForceMode2D.Impulse);
                        mob.ReceiveDamage(new DamageMessage(_mob, mob, 10 * (1 - (mob.Position - _mob.Position).MagnetudeXY() / _radius), _source, .1f));
                    }
                }
                var go = PrefabCreator.Instantiate(_explosion.Key, _mob.GroundCenterPosition);
            });
        }
    }

    public override void Shutdown()
    {
        //_enebled = false;
    }
}
