using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkUIonizationEffect : WithId, IPerk
{
    public PerkType Type => PerkType.IonizationEffect;

    public bool IsCommon => false;

    protected float Damage;

    private IMob _attacker;
    private IMob _mob;
    private IDisposable _loop;
    public float Countdown { get; private set; }
    private Fx _fx = new Fx("Fx_Radiation", FxPosition.SpriteMesh);

    public PerkUIonizationEffect(IMob attaker, float damage, float time)
    {
        _attacker = attaker;
        Damage = damage;
        Countdown = time;
    }

    public void Add(IPerk perk)
    {
        PerkUIonizationEffect effect = ((PerkUIonizationEffect)perk);
        //if (effect.Damage > Damage)
        Countdown = effect.Countdown;
        Damage += effect.Damage;
    }

    public void Init(IMob mob)
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

    public void Shutdown()
    {
        _loop.Dispose();
        _mob.RemoveFx(_fx);
    }

    public string GetDescription()
    {
        throw new NotImplementedException();
    }
}
