using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Umb16.Extensions;
using Random = UnityEngine.Random;

public class PerkEMother : PerkEStandart
{
    private int _maxCount = 5;
    private MobSpawner _spawner;
    private List<IMob> _spawnedMobs = new List<IMob>();
    private AddressKeys _mobSpawnKey = AddressKeys.Mob_Child;

    private IDisposable _updateDisposible;

    public PerkEMother(int maxCount, AddressKeys mobSpawnKey)
    {
        _maxCount = maxCount;
        _mobSpawnKey = mobSpawnKey;
    }

    private int Count => _spawnedMobs.Count;

    public override PerkType Type => PerkType.EMother;

    public override void Init(IMob mob)
    {
        base.Init(mob);
        _spawner = _mob.Spawner;
        mob.DamageController.Die += OnSomeoneDie;
        _updateDisposible = UniTaskAsyncEnumerable.Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1)).SubscribeAwait(Update);
    }

    private void OnSomeoneDie(DamageMessage msg)
    {
        for (int i = 0; i < _spawnedMobs.Count; i++)
        {
            if (_spawnedMobs[i] == msg.Target)
            {
                _spawnedMobs.RemoveAt(i);
                i--;
            }
        }
    }

    private async UniTask Update(AsyncUnit obj)
    {
        if (_maxCount > Count)
        {
            Vector3 pos = Vector3.up.DiamondRotateXY(4.0f/_maxCount* _spawnedMobs.Count);
            IMob mob = await _spawner.SpawnByName(AddressKeysConverter.Convert(_mobSpawnKey), _mob.Position + pos);
            _spawnedMobs.Add(mob);
        }
    }
    public override void Shutdown()
    {
        base.Shutdown();
        _mob.DamageController.Die -= OnSomeoneDie;
        _updateDisposible.Dispose();
    }
}
