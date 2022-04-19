using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class PerkPBarrels : WithId, IPerk
{
    private IMob _mob;

    public PerkType Type => PerkType.PBarrels;
    private float Cooldown => 25;
    private float _lastSpawnTime;
    private float NextSpawnTime => _lastSpawnTime + Cooldown;
    private bool SpawnTimeCome => NextSpawnTime < Time.time;

    public bool IsCommon => false;

    private IDisposable _loop;
    public PerkPBarrels()
    {
        _loop = UniTaskAsyncEnumerable.EveryValueChanged(this, x => x.SpawnTimeCome)
            .Where(x => x == true)
            .Subscribe(async _ => await CreateBarrel());
    }
    public void Init(IMob mob)
    {
        _mob = mob;
    }

    private async UniTask CreateBarrel()
    {
        _lastSpawnTime = Time.time;
        var go = await PrefabCreator.Instantiate("Obj_Barrel", new Vector3(Random.value * 2 - 1, Random.value * 2 - 1).normalized * 30 * Random.value);
        _mob.AllMobs.Add(go.GetComponent<IMob>());
    }

    public void Shutdown()
    {
        _loop.Dispose();
    }

    public void Add(IPerk perk)
    {
        
    }
}
