using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks;
using Zenject;
using System.Threading;
using Umb16.Extensions;

public class Mob : MonoBehaviour, IMovable
{
    public ComplexStat Speed { get; private set; }
    public ComplexStat HP { get; protected set; }

    private List<IPerk> _perks = new List<IPerk>();
    public StandartStatsCollection StatsCollection { get; protected set; }
    public DamageController DamageController { get; private set; }
    public GameData GameData { get; private set; }
    public Player Player { get; private set; }
    public bool IsReady { get; private set; }
    private Vector3 _moveTarget;
    private bool _stopped = true;
    private SpriteRenderer _sprite;

    // virtual public void Init() { }
    [Inject]
    private void Construct(DamageController damageController, GameData gameData, Player player)
    {
        GameData = gameData;
        damageController.Damage += ReceiveDamage;
        DamageController = damageController;
        Player = player;
        IsReady = true;
    }

    public void AddPerk(Func<Mob, IPerk> perkGenerator, int level = 0)
    {
        var perk = perkGenerator(this);
        _perks.Add(perk);
        if(level>0)
            perk.SetLevel(level);
    }

    private void ReceiveDamage(DamageMessage message)
    {
        HP.AddBaseValue(-message.Damage);
        if (HP.Value <= 0)
        {
            Die(message);
        }
    }

    public void Die(DamageMessage message)
    {
        Stop();
        DamageController.SendDie(message);
    }

    public void MoveTo(Vector3 target)
    {
        _moveTarget = target;
        _stopped = false;
    }

    public void Stop()
    {
        _stopped = true;
    }

    private void Start()
    {
        Speed = StatsCollection.GetStat(StatType.Speed);
        HP = StatsCollection.GetStat(StatType.HP);
        var size = StatsCollection.GetStat(StatType.Size);
        transform.localScale = Vector3.one * size.Value;
        size.ValueChanged += (x) =>
        transform.localScale = Vector3.one * x.Value;
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (!_stopped)
        {
            if (!transform.position.EqualsWithThreshold(_moveTarget, .1f))
            {
                //������������� �������� � ����������� �� ����������� ��������
                _sprite.flipX = _moveTarget.x - transform.position.x < 0;

                Vector3 pos = transform.position;
                pos += Speed.Value * Time.deltaTime * (_moveTarget - transform.position).normalized;
                pos.z = pos.y * .1f;
                transform.position = pos;
            }
        }
    }
}
