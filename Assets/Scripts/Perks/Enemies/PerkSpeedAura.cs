using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Umb16.Extensions;
using UnityEngine;

public class PerkSpeedAura : IPerk
{
    public PerkType Type => PerkType.ESpeedAura;
    public string Name => throw new System.NotImplementedException();

    public string Description => throw new System.NotImplementedException();

    private IMob _mob;

    public int Level => 1;

    public bool Maxed => true;

    public int MaxLevel => 1;

    private HashSet<IMob> _mobsInRadius = new HashSet<IMob>();


    private float _distanceValue = 12;

    private Fx _aura = new Fx("Fx_Aura0", FxPosition.Ground);
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    public PerkSpeedAura(IMob mob)
    {
        SetParent(mob);
        mob.AddFx(_aura);
    }

    public void AddLevel()
    {
        Debug.Log("Is static perk");
    }

    public void SetLevel(int level)
    {
        Debug.Log("Is static perk");
    }

    public void SetParent(IMob mob)
    {
        _mob = mob;
        StartUpdate().AttachExternalCancellation(_cancellationTokenSource.Token);
    }

    private async UniTask StartUpdate()
    {
        await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate())
        {
            foreach (var mob in _mob.AllMobs)
            {
                if (mob == _mob)
                    continue;
                if ((_mob.Position - mob.Position).SqrMagnetudeXY() < _distanceValue * _distanceValue)
                {
                    if (!_mobsInRadius.Contains(mob) && !mob.ContainPerk(PerkType.ESpeedAuraEffect))
                    {
                        OnEnter(mob);
                    }
                }
                else
                {
                    if (_mobsInRadius.Contains(mob))
                    {
                        OnExit(mob);
                    }
                }
            }
        }
    }

    private void RemoveAll()
    {
        foreach (var mob in _mobsInRadius)
        {
            if (mob != null && !mob.IsDead)
                mob.RemovePerk(PerkType.ESpeedAuraEffect);
        }
        _mob.RemoveFx(_aura);
        //_mobsInRadius.Clear();
    }

    private void OnExit(IMob mob)
    {
        _mobsInRadius.Remove(mob);
        mob.RemovePerk(PerkType.ESpeedAuraEffect);
    }

    private void OnEnter(IMob mob)
    {
        _mobsInRadius.Add(mob);
        mob.AddPerk((x) => new PerkSpeedAuraEffect(x));
    }

    public void Shutdown()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        RemoveAll();
    }
}
