using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using Cysharp.Threading.Tasks.Linq;
using UnityEditor.Localization.Plugins.XLIFF.V12;

public class PerkStarDust : PerkGravityMatter
{
    private List<OrbitalProjectile> _shards = new();
    private IDisposable _loop;
    private float _lifetime;
    private int _maxShards;
    public PerkStarDust(int radius, float speed, int damage, float stunTime, float size, float lifetime, int maxShards, PerkType perkType) : base(radius, speed, damage, stunTime, size, perkType)
    {
        _lifetime = lifetime;
        _maxShards = maxShards;
    }
    public override void Add(IPerk perk)
    {

    }
    public override void Init(IMob mob)
    {
        _mob = mob;
        _mob.PickedUpScoreGem += OnPickingUpScoreGem;
        _loop = UniTaskAsyncEnumerable.Timer(TimeSpan.FromSeconds(.1f), TimeSpan.FromSeconds(.1f)).Subscribe(Update);
    }
    public void OnPickingUpScoreGem()
    {
        if (_shards.Count < _maxShards)
            CreateGravityStone().Forget();
    }
    public override async UniTask CreateGravityStone()
    {
        var go = await PrefabCreator.Instantiate(Addresses.Obj_GravityStone, Vector3.zero);
        OrbitalProjectile _newStone = go.GetComponent<OrbitalProjectile>();

        _shards.Add(_newStone);
        PhaseTimeReCalculate();

        _newStone.SetParams(_radius, _speed, _damage, _stunTime, _perkType);
        _newStone.Init(_mob);
        _newStone.Lifetime = _lifetime;

    }
    private void PhaseTimeReCalculate()
    {
        float _phasePart = Mathf.PI * 2 / _shards.Count;

        for (int i = 0; i < _shards.Count; i++)
        {
            _shards[i].SetPhaseTime(i * _phasePart);
        }

    }
    public override void Shutdown()
    {
        _loop.Dispose();
        _mob.PickedUpScoreGem -= OnPickingUpScoreGem;

        foreach (var item in _shards)
        {
            if (item != null)
                GameObject.Destroy(item.gameObject);
        }
        _shards.Clear();
    }
    private void Update(AsyncUnit obj)
    {
        for (int i = 0; i < _shards.Count; i++)
        {
            if ((_shards[i].Lifetime -= .1f) <= 0)
            {
                GameObject.Destroy(_shards[i].gameObject);
                _shards.RemoveAt(i);
                PhaseTimeReCalculate();
                i--;
            }
        }
    }
}
