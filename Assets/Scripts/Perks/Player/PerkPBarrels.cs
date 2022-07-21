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
    private UICooldownsManager _cooldownsPanel;

    public PerkPBarrels(UICooldownsManager cooldownsManager, float cooldown)
    {
        _cooldownsPanel = cooldownsManager;
        _cooldown = cooldown;
    }
    public void Init(IMob mob)
    {
        _lastSpawnTime = Time.time;
        _mob = mob;
        _loops.Add(UniTaskAsyncEnumerable.EveryValueChanged(this, x => x.SpawnTimeCome)
        .Where(x => x == true)
        .Subscribe(async _ => await CreateBarrel()));
        _cooldownsPanel.AddIndiacator(Addresses.Ico_Box).ContinueWith(x=>
        {
            _indicator = x;
            _loops.Add(UniTaskAsyncEnumerable.EveryUpdate().Subscribe(_ =>
            {
                _indicator.SetTime(NextSpawnTime - Time.time, _cooldown);
            }
            ));
        }).Forget();
    }

    private async UniTask CreateBarrel()
    {
        _lastSpawnTime = Time.time;
        var go = await PrefabCreator.Instantiate("Obj_Barrel", new Vector3(Random.value * 2 - 1, Random.value * 2 - 1).normalized * 30 * Random.value);
        go.transform.eulerAngles -= Vector3.right * 90;
        _mob.AllMobs.Add(go.GetComponent<IMob>());
    }

    public void Shutdown()
    {
        foreach (var loop in _loops)
        {
            loop.Dispose();
        }
        _indicator?.Destroy();
    }

    public void Add(IPerk perk)
    {

    }

    public string GetDescription()
    {
        return "Описание";
    }
}
