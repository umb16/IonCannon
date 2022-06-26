using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEvent : LevelEvent
{
    private string _address;
    private float _minSpawnDelay;
    private float _maxSpawnDelay;
    private bool _started = false;
    private float _nextSpawnTime = 0;
    private int _fixedCount = 0;
    private float? _direction = null;
    private float _directionError = 0;
    private int _count;

    public SpawnEvent(float startInseconds, float endInseconds, string address, float minSpawnDelay, float maxSpawnDelay) : base(startInseconds, endInseconds)
    {
        _address = address;
        _minSpawnDelay = minSpawnDelay;
        _maxSpawnDelay = maxSpawnDelay;
    }
    public SpawnEvent(float startInseconds, float endInseconds, string address, float spawnDelay) : base(startInseconds, endInseconds)
    {
        _address = address;
        _minSpawnDelay = spawnDelay;
        _maxSpawnDelay = spawnDelay;
    }
    public SpawnEvent(float startInseconds, string address) : base(startInseconds, startInseconds)
    {
        _address = address;
        _minSpawnDelay = 0;
        _maxSpawnDelay = 0;
        _fixedCount = 1;
    }
    public SpawnEvent SetFixedCount(int fixedCount)
    {
        _fixedCount = fixedCount;
        return this;
    }
    public SpawnEvent SetDirection(float direction, float error = 0)
    {
        _direction = direction;
        _directionError = error;
        return this;
    }
    public override void Update()
    {

        if (_mobSpawner != null)
        {
            if (_mobSpawner.GameData.GameTime > EndInseconds && _started && (_fixedCount == 0 || _fixedCount - _count == 0) || (_fixedCount > 0 && _fixedCount - _count == 0))
                return;
            if (_mobSpawner.GameData.GameTime > StartInseconds && !_started)
            {
                _started = true;
                _nextSpawnTime = Time.time;
            }
            if (_nextSpawnTime <= Time.time && _started)
            {
                _nextSpawnTime += Random.Range(_minSpawnDelay, _maxSpawnDelay);
                if (_direction == null)
                {
                    _mobSpawner.SpawnByName(_address, _mobSpawner.GetRandomSpawnPoint()).Forget();
                }
                else
                {
                    _mobSpawner.SpawnByName(_address, _mobSpawner.GetSpawnPointFromDirection(_direction.Value + _directionError * (Random.value - .5f))).Forget();
                }
                _count++;
            }
        }
    }

    public override void Reset()
    {
        _started = false;
        _count = 0;
    }
}
