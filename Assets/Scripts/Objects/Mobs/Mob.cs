using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks;
using Zenject;
using System.Threading;
using Umb16.Extensions;

public class Mob : MonoBehaviour, IMob
//: MonoBehaviour//, IMovable
{
    private static int idIndex;
    [SerializeField] bool _mirroringOnMove = true;
    public int ID { get; private set; }
    public ComplexStat MovementSpeed { get; private set; }
    public ComplexStat HP { get; protected set; }

    public bool IsDead => HP.Value <= 0;

    private Dictionary<PerkType, IPerk> _perks = new Dictionary<PerkType, IPerk>();
    public StandartStatsCollection StatsCollection { get; protected set; }
    public DamageController DamageController { get; private set; }
    public GameData GameData { get; private set; }
    public Player Player { get; private set; }
    public bool IsReady { get; private set; }

    public Vector3 Position => transform.position;

    private Vector3 _moveTarget;
    private bool _stopped = true;

    [Inject]
    private void Construct(DamageController damageController, GameData gameData, Player player)
    {
        GameData = gameData;
        DamageController = damageController;
        Player = player;
        IsReady = true;
        ID = ++idIndex;
    }

    public void AddPerk(Func<Mob, IPerk> perkGenerator, int level = 0)
    {
        var perk = perkGenerator(this);
        _perks.Add(perk.Type, perk);
        if (level > 0)
            perk.SetLevel(level);
    }

    public virtual void ReceiveDamage(DamageMessage message)
    {
        if (IsDead)
            return;
        DamageController.SendDamage(message);
        HP.AddBaseValue(-message.Damage);
        if (IsDead)
        {
            Die(message);
        }
    }

    public virtual void Die(DamageMessage message)
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
        MovementSpeed = StatsCollection.GetStat(StatType.MovementSpeed);
        HP = StatsCollection.GetStat(StatType.HP);
        var size = StatsCollection.GetStat(StatType.Size);
        transform.localScale = Vector3.one * size.Value;
        transform.To2DPos();
        size.ValueChanged += (x) => transform.localScale = Vector3.one * x.Value;
    }

    private void FixedUpdate()
    {
        if (!_stopped)
        {
            if (!transform.position.EqualsWithThreshold(_moveTarget, .1f))
            {
                if (_mirroringOnMove)
                    Flip();

                Vector3 pos = transform.position;
                pos += MovementSpeed.Value * Time.deltaTime * (_moveTarget - pos).normalized;
                transform.position = pos.Get2D();
            }
        }
    }

    private void Flip()
    {
        var scale = transform.localScale;
        if (_moveTarget.x - transform.position.x < 0)
        {

            scale.x = -1;

        }
        else
        {
            scale.x = 1;
        }
        transform.localScale = scale;
    }

    public void SetPosition(float x, float y)
    {
        transform.Set2DPos(x, y);
    }

    public void SetPosition(Vector3 vector)
    {
        transform.Set2DPos(vector.x, vector.y);
    }
}
