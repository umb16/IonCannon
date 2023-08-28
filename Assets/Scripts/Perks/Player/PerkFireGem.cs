using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PerkFireGem : PerkBase
{
    private IMob _mob;
    private List<IDisposable> _loops = new();
    private int _projectileCount;
    private float _cooldown;
    private CooldownIndicator _indicator;
    private UICooldownsManager _cooldownsPanel;
    private float _lastSpawnTime;
    private float NextSpawnTime => _lastSpawnTime + _cooldown;

    public PerkFireGem(int projectailCount, float cooldown, UICooldownsManager cooldownsPanel)
    {
        _projectileCount = projectailCount;
        _cooldown = cooldown;
        _cooldownsPanel = cooldownsPanel;
    }

    public override void Add(IPerk perk)
    {
        throw new System.NotImplementedException();
    }

    public override void Init(IMob mob)
    {
        _lastSpawnTime = Time.time;
        _mob = mob;
        CreateLoops();
    }

    public void CreateLoops()
    {
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

        float _phasePart = Mathf.PI * 2 / _projectileCount;

        for (int i = 0; i < _projectileCount; i++)
        {
            CreateFireProjectiles(i * _phasePart).Forget();
        }
        
    }
    public virtual async UniTask CreateFireProjectiles(float phaseTime)
    {
        var go = await PrefabCreator.Instantiate(Addresses.Obj_FireSpark, Vector3.zero);
        FireSpark fireObj = go.GetComponent<FireSpark>();
        fireObj.SetParams(20, phaseTime, _mob);
        fireObj.Init();
    }

}
