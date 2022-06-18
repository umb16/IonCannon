using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Umb16.Extensions;
using UnityEngine;

public class PerkEAfterDeathExplosion : PerkEStandart
{
    public float Radius = 10;
    public float Delay = .49f;
    private DamageSources _source = DamageSources.Explosion;
    public override PerkType Type => PerkType.EAfterDeathExplosion;
    private Fx _explosion = new Fx("Fx_Explosion", FxPosition.Ground);
    private Timer _timer;
    private bool _disabled;


    private void OnDie(DamageMessage msg)
    {
        if(_disabled)
            return;
        if (msg.Target == _mob)
        {
            _mob.DamageController.Die -= OnDie;
            ((Mob)_mob).GetComponent<Animator>().speed = 3;
            _timer = new Timer(Delay)
            .SetEnd(() =>
            {
                _mob.GameData.GameStateChanged -= GameStateChanged;
                List<IMob> mobs = new List<IMob>(_mob.AllMobs);
                mobs.Add(_mob.Player);
                for (int i = 0; i < mobs.Count; i++)
                {
                    var mob = mobs[i];
                    if (mob == _mob)
                        continue;
                    if ((mob.Position - _mob.Position).SqrMagnetudeXY() < Radius * Radius)
                    {
                        mob.AddForce((1 - (mob.Position - _mob.Position).MagnetudeXY() / Radius) * (mob.Position - _mob.Position).NormalizedXY() * 5, ForceMode2D.Impulse);
                        mob.ReceiveDamage(new DamageMessage(_mob, mob, 100 * (1 - (mob.Position - _mob.Position).MagnetudeXY() / Radius), _source, .1f));
                    }
                }
                var go = PrefabCreator.Instantiate(_explosion.Key, _mob.GroundCenterPosition);
            });
        }
    }

    public override void Init(IMob mob)
    {
        base.Init(mob);
        _mob.DamageController.Die += OnDie;
        _mob.GameData.GameStateChanged += GameStateChanged;
    }

    private void GameStateChanged(GameState state)
    {
        if (state == GameState.Restart)
        {
            _timer?.Stop();
            _mob.GameData.GameStateChanged -= GameStateChanged;
            _disabled = true;
        }
    }

    public override void Shutdown()
    {
        base.Shutdown();
        
        //_enebled = false;
    }
}
