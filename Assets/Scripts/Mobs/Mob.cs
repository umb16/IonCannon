using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks;

public class Mob : MonoBehaviour, IMovable
{
    private List<IPerk> _perks = new List<IPerk>();
    public StandartStatsCollection StatsCollection { get; private set; }

    private Vector3 _moveTarget;
    private bool _stopped;

    public void AddPerk(Func<IStatsCollection, IPerk> perkGenerator)
    {
        _perks.Add(perkGenerator(StatsCollection));
    }

    public void ReceiveDamage(DamageSources source, float value)
    {
        StatsCollection.GetStat(StatType.HP).AddBaseValue(-value);
        foreach (var perk in _perks)
        {
            perk.OnReceiveDamage(source);
        }
    }

    public void MoveTo(Vector3 target)
    {
        _moveTarget = target;
        _stopped = false;
    }

    public void Stop()
    {
        _stopped = true;
    }


    private async UniTask Start()
    {
        await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate())
        {
            OnUpdate();
        }
    }

    private void OnUpdate()
    {
        if (!_stopped)
        {
            transform.position += (_moveTarget - transform.position).normalized * Time.deltaTime * StatsCollection.GetStat(StatType.Speed).Value;
        }
        foreach (var perk in _perks)
        {
            perk.OnUpdate();
        }
    }
}
