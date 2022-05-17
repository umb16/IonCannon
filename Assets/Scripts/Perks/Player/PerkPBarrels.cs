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
    private float _cooldown = 20;
    private float _lastSpawnTime;
    private float NextSpawnTime => _lastSpawnTime + _cooldown;
    private bool SpawnTimeCome => NextSpawnTime < Time.time;

    public bool IsCommon => true;
    private CooldownIndicator _indicator;

    private List<IDisposable> _loops = new List<IDisposable>();
    private CooldownsPanel _cooldownsPanel;

    public PerkPBarrels(CooldownsPanel cooldownsPanel, float cooldown)
    {
        _cooldownsPanel = cooldownsPanel;
        _cooldown = cooldown;
    }
    public void Init(IMob mob)
    {
        _lastSpawnTime = Time.time;
        _indicator = _cooldownsPanel.AddIndiacator(AddressKeys.Ico_Box);
        _mob = mob;
        _loops.Add(UniTaskAsyncEnumerable.EveryValueChanged(this, x => x.SpawnTimeCome)
        .Where(x => x == true)
        .Subscribe(async _ => await CreateBarrel()));
        _loops.Add(UniTaskAsyncEnumerable.EveryUpdate().Subscribe(_ =>
        {
            _indicator.SetTime(NextSpawnTime - Time.time, _cooldown);
        }
        ));
    }

    private async UniTask CreateBarrel()
    {
        _lastSpawnTime = Time.time;
        var go = await PrefabCreator.Instantiate("Obj_Barrel", new Vector3(Random.value * 2 - 1, Random.value * 2 - 1).normalized * 30 * Random.value);
        _mob.AllMobs.Add(go.GetComponent<IMob>());
    }

    public void Shutdown()
    {
        foreach (var loop in _loops)
        {
            loop.Dispose();
        }
        _indicator.Destroy();
    }

    public void Add(IPerk perk)
    {

    }
}
