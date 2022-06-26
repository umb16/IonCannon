using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelEvent
{
    public float StartInseconds { get; private set; }
    public float EndInseconds { get; private set; }

    protected MobSpawner _mobSpawner;

    public LevelEvent(float startInseconds, float endInseconds)
    {
        StartInseconds = startInseconds;
        EndInseconds = endInseconds;
    }
    public void SetSpawner(MobSpawner mobSpawner)
    {
        _mobSpawner = mobSpawner;
    }

    public abstract void Update();
    public abstract void Reset();
}
