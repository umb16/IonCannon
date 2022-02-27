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
        StatsCollection.SetStat(StatType.MovementSpeed, _speed);
        StatsCollection.SetStat(StatType.Score, _score);
        StatsCollection.SetStat(StatType.Size, _size);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ray" || collision.gameObject.tag == "Barrel")
        {
            ReceiveDamage(new DamageMessage(Player, this, Player.RayDmg * Time.deltaTime*2, DamageSources.Ray));
            //Destroy(gameObject);
            //Destroy(UnityEngine.Object.Instantiate(Blood, transform.position + Vector3.back * 0.5f, Blood.transform.rotation), 10f);
        }
    }
    private void Update()
    {
        if (Player != null && Player.transform != null)
            MoveTo(Player.transform.position);
    }
}
