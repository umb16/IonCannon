using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ConsoleMethods : MonoBehaviour
{
    private AsyncReactiveProperty<Player> _player;
    private MobSpawner _mobSpawner;
    private GameData _gameData;

    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player, MobSpawner mobSpawner, GameData gameData)
    {
        _player = player;
        _mobSpawner = mobSpawner;
        _gameData = gameData;
    }

    private void Start()
    {
        OnGUIConsole.Instance.AddMethod("AddStat", (Action<StatType, float>)AddStat);
        OnGUIConsole.Instance.AddMethod("SpawnerStop", (Action<int>)SpawnerStop);
        OnGUIConsole.Instance.AddMethod("KillUnderCursor", (Action)KillUnderCursor);
        OnGUIConsole.Instance.AddMethod("SpawnMob", (Action<string>)SpawnMob);
        OnGUIConsole.Instance.AddMethod("AddWave", (Action)AddWave);
    }

    private void AddStat(StatType type, float value)
    {
        Debug.Log("Add "+type+" "+value);
        var stat = _player.Value.StatsCollection.GetStat(type);
        stat.SetBaseValue(stat.BaseValue + value);
    }

    private void SpawnerStop(int stop)
    {
        _mobSpawner.Stop = stop > 0;
    }

    private void SpawnMob(string name)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mobSpawner.SpawnByName(name, pos).Forget();
    }
    private void AddWave()
    {
        _mobSpawner.NextWave();
        //_gameData.AddWave();
    }
    private void KillUnderCursor()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit.transform != null)
        {
            var mob = hit.transform.GetComponent<IMob>();
            if (mob != null)
                mob.ReceiveDamage(new DamageMessage(_player.Value, mob,9999, DamageSources.Unknown));
        }
    }
}
