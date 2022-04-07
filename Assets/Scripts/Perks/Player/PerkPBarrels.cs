using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class PerkPBarrels : IPerk
{
    private IMob _mob;

    public PerkType Type => PerkType.PBarrels;

    public string Name => LocalizationManager.Instance.GetPhrase(LocKeys.ExplosiveBox);

    public string Description => LocalizationManager.Instance.GetPhrase(LocKeys.ExplosiveBox);

    public int Level { get; private set; }

    public bool Maxed => Level == MaxLevel;

    public int MaxLevel => 3;
    private float Cooldown => 30 - Level * 5;
    private float _lastSpawnTime;
    private float NextSpawnTime => _lastSpawnTime + Cooldown;
    private bool SpawnTimeCome => Level > 0 && NextSpawnTime < Time.time;
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
    public void AddLevel()
    {
        Level++;
    }

    public void SetLevel(int level)
    {
        Level = level;
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
