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
    private ItemsDB _itemsDB;

    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player, MobSpawner mobSpawner, GameData gameData,
        ShopShip shopShip, ItemsDB itemsDB)
    {
        _player = player;
        _mobSpawner = mobSpawner;
        _gameData = gameData;
        _shopShip = shopShip;
        _itemsDB = itemsDB;
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
        if (TryParseType<StatType>(type, out var sType))
        {
            var stat = _player.Value.StatsCollection.GetStat(sType);
            stat.SetBaseValue(stat.BaseValue + value);
        }
    }

    public void AddGold(float value)
    {
        var stat = _player.Value.Gold;
        stat.SetBaseValue(stat.BaseValue + value);
    }
    private bool TryParseType<T>(string type, out T sType) where T : struct
    {
        if (Enum.TryParse<T>(type, true, out sType))
        {
            return true;
        }
        Debug.LogWarning("Not valid type: " + type + " in " + typeof(T).Name);
        return false;
    }

    public void AddItem(string type)
    {
        if (TryParseType<ItemType>(type, out var sType))
        {
            var newItem = _itemsDB.CreateByType(sType);
            _player.Value.AddItemDirectly(newItem);
        }
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
