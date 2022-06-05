using Cysharp.Threading.Tasks;
using MiniScriptSharp.Inject;
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
    private ShopShip _shopShip;

    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player, MobSpawner mobSpawner, GameData gameData,
        ShopShip shopShip)
    {
        _player = player;
        _mobSpawner = mobSpawner;
        _gameData = gameData;
        _shopShip = shopShip;
        FunctionInjector.AddFunctions(this, Debug.Log);
    }

    private void Start()
    {
        // OnGUIConsole.Instance.AddMethod("AddStat", (Action<StatType, float>)AddStat);
        //OnGUIConsole.Instance.AddMethod("SpawnerStop", (Action<int>)SpawnerStop);
        OnGUIConsole.Instance.AddMethod("KillUnderCursor", (Action)KillUnderCursor);
        OnGUIConsole.Instance.AddMethod("SpawnMob", (Action<string>)SpawnMob);
        OnGUIConsole.Instance.AddMethod("AddWave", (Action)AddWave);
    }


    public void AddStat(string type, float value)
    {
        Debug.Log("Add " + type + " " + value);
        if (Enum.TryParse<StatType>(type, true, out var sType))
        {
            var stat = _player.Value.StatsCollection.GetStat(sType);
            stat.SetBaseValue(stat.BaseValue + value);
        }
        else
        {
            Debug.LogWarning("Not valid type");
        }
    }

    /*private bool TryParseType<T>(string type, out TEnum sType) where T : TEnum
    {
        Enum.TryParse<T>(type, true, out sType)
    }*/

    public void AddItem(string itemType)
    {
        
    }

    public void SetShopShipTime(float shift)
    {
        _shopShip.SetLastArrival(shift);
    }

    public void SpawnerStop(bool stop)
    {
        _mobSpawner.Stop = stop;
        Debug.Log("spawnerStop " + stop);
    }

    public void SpawnMob(string name)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mobSpawner.SpawnByName(name, pos).Forget();
    }
    public void AddWave()
    {
        _mobSpawner.NextWave();
        //_gameData.AddWave();
    }
    public void KillUnderCursor()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit.transform != null)
        {
            var mob = hit.transform.GetComponent<IMob>();
            if (mob != null)
                mob.ReceiveDamage(new DamageMessage(_player.Value, mob, 9999, DamageSources.Unknown));
        }
    }
}
