using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PerkElectricalLead : PerkBase
{
    private IMob _mob;
    private List<IDisposable> _loops = new();
    private List<IMob> _mobs = new();
    private float _cooldown;
    private CooldownIndicator _indicator;
    private UICooldownsManager _cooldownsPanel;
    private float _lastSpawnTime;
    private float _radius = 20;
    private float NextSpawnTime => _lastSpawnTime + _cooldown;

    public PerkElectricalLead(UICooldownsManager cooldownsPanel, float cooldown)
    {
        _cooldown = cooldown;
        _cooldownsPanel = cooldownsPanel;
    }

    public override void Add(IPerk perk)
    {
        
    }

    public override void Init(IMob mob)
    {
        _lastSpawnTime = Time.time;
        _mob = mob;
        _mobs = mob.AllMobs;
        _loops.Add(UniTaskAsyncEnumerable.Timer(TimeSpan.FromSeconds(_cooldown), TimeSpan.FromSeconds(_cooldown)).Subscribe(Update));

        _cooldownsPanel.AddIndiacator(Addresses.Ico_ElectricalLead).ContinueWith(x =>
        {
            _indicator = x;
            _loops.Add(UniTaskAsyncEnumerable.EveryUpdate().Subscribe(_ =>
            {
                _indicator.SetTime(NextSpawnTime - Time.time, _cooldown);
            }
            ));
        }).Forget();
    }

    public override void Shutdown()
    {
        foreach (var loop in _loops)
        {
            loop.Dispose();
        }
        _indicator?.Destroy();
    }

    private void Update(AsyncUnit obj)
    {
        _lastSpawnTime = Time.time;

        CreateLightning().Forget();
    }
    public virtual async UniTask CreateLightning()
    {
        var go = await PrefabCreator.Instantiate(Addresses.Obj_Lightning, Vector3.zero);
        LightningBuilder lightning = go.GetComponent<LightningBuilder>();
        lightning.Init(_mob);
    }
}
