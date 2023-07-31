using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PerkFrostbiteEffect : WithId, IPerk
{
    private IMob _mob;
    private IDisposable _loop;
    private StatModificator _slowdown;
    public float Countdown { get; private set; }

    public PerkFrostbiteEffect()
    {
        _slowdown = new StatModificator(-.7f, StatModificatorType.Multiplicative, StatType.MovementSpeed);
        Countdown = 4;
    }

    public bool IsCommon => false;

    public PerkType Type => PerkType.FrostbiteEffect;

    public void Add(IPerk perk)
    {
        PerkFrostbiteEffect effect = ((PerkFrostbiteEffect)perk);
        Countdown = effect.Countdown;
    }

    public string GetDescription()
    {
        return "";
    }

    public void Init(IMob mob)
    {
        _mob = mob;
        _mob.StatsCollection.AddModificator(_slowdown);
        _loop = UniTaskAsyncEnumerable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)).Subscribe(Update);
    }

    public void Shutdown()
    {
        _loop.Dispose();
    }
    private void Update(AsyncUnit obj)
    {
        Countdown--;

        if (Countdown <= 0)
        {
            _mob.StatsCollection.RemoveModificator(_slowdown);
            _mob.RemovePerk(this);
        }           
    }
}
