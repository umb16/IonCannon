using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkUIonizationEffect : PerkBase
{
    protected float Damage;

    private IMob _attacker;
    private IMob _mob;
    private IDisposable _loop;
    public float Countdown { get; private set; }
    private Fx _fx = new Fx("Fx_Radiation", FxPosition.SpriteMesh);

    public PerkUIonizationEffect(IMob attacker, float damage, float time)
    {
        _attacker = attacker;
        Damage = damage;
        Countdown = time;
        SetType(PerkType.IonizationEffect);
    }

    public override void Add(IPerk perk)
    {
        PerkUIonizationEffect effect = ((PerkUIonizationEffect)perk);
        Countdown = effect.Countdown;
        Damage += effect.Damage;
    }

    public override void Init(IMob mob)
    {
        _mob = mob;
        _loop = UniTaskAsyncEnumerable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)).Subscribe(Update);
        _mob.AddFx(_fx).Forget();
    }

    private void Update(AsyncUnit obj)
    {
        _mob.ReceiveDamage(new DamageMessage(_attacker, _mob, Damage, DamageSources.Ionization));
        Countdown--;
        if (Countdown <= 0)
            _mob.RemovePerk(this);
    }

    public override void Shutdown()
    {
        _loop.Dispose();
        _mob.RemoveFx(_fx);
    }
}
