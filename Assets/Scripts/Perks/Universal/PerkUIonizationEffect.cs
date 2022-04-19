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
    private Fx _fx = new Fx("Fx_Radiation", FxPosition.SpriteMesh);

    public PerkUIonizationEffect(IMob attaker, float damage)
    {
        _attacker = attaker;
        Damage = damage;
    }

    public void Add(IPerk perk)
    {
        PerkUIonizationEffect effect = ((PerkUIonizationEffect)perk);
        if (effect.Damage > Damage)
            Damage = effect.Damage;
    }

    public void Init(IMob mob)
    {
        _mob = mob;
        _loop = UniTaskAsyncEnumerable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)).Subscribe(Update);
        _mob.AddFx(_fx);
    }

    private void Update(AsyncUnit obj)
    {
        _mob.ReceiveDamage(new DamageMessage(_attacker, _mob, Damage, DamageSources.Ionization));
    }

    public void Shutdown()
    {
        _loop.Dispose();
        _mob.RemoveFx(_fx);
    }
}
