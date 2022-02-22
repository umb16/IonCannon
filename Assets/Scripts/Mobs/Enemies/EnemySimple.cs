using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimple : Mob
{
    [SerializeField] private float _hp = 1;
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _score = 1;
    [SerializeField] private float _size = 1;
    private void Awake()
    {
        StatsCollection = StatsCollectionsDB.StandartEnemy();
        StatsCollection.SetStat(StatType.MaxHP, _hp);
        StatsCollection.SetStat(StatType.HP, _hp);
        StatsCollection.SetStat(StatType.Speed, _speed);
        StatsCollection.SetStat(StatType.Score, _score);
        StatsCollection.SetStat(StatType.Size, _size);
    }
    private void Update()
    {
        if (Player?.transform != null)
            MoveTo(Player.transform.position);
    }
}
