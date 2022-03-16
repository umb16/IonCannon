using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks;
using Zenject;
using System.Threading;
using Umb16.Extensions;

public class Mob : MonoBehaviour, IMob
{
    private static int idIndex;
    [SerializeField] bool _mirroringOnMove = true;
    [SerializeField] private Transform _groundCenterPoint;
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
    public HashSet<IMob> AllMobs { get; private set; }

    private Vector3 _moveTarget;
    private bool _stopped = true;

    private MobFxes _mobFxes = new MobFxes();

    [Inject]
    private void Construct(DamageController damageController, GameData gameData, Player player, MobSpawner mobSpawner)
    {
        GameData = gameData;
        DamageController = damageController;
        Player = player;
        IsReady = true;
        ID = ++idIndex;
        AllMobs = mobSpawner.Mobs;
    }

    public void AddPerk(Func<IMob, IPerk> perkGenerator, int level = 0)
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
        OnDie();
        DamageController.SendDie(message);
    }

    public void MoveTo(Vector3 target)
    {
        _moveTarget = target;
        _stopped = false;
    }

    public void OnDie()
    {
        _stopped = true;
        ShutdownPerks();
    }

    private void ShutdownPerks()
    {
        foreach (var perk in _perks)
        {
            perk.Value.Shutdown();
        }
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

    public async UniTask AddFx(Fx fx)
    {
       Transform parent;
        switch (fx.FxPosition)
        {
            case FxPosition.Ground:
                parent = _groundCenterPoint;
                break;
            default:
                parent = transform;
                break;
        }
        var go = await PrefabCreator.GetInstance(fx.Key, parent).AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
        go.transform.localPosition = Vector3.zero;
        _mobFxes.Add(fx, go);
    }

    public void RemoveFx(Fx fx)
    {
        _mobFxes.Remove(fx);
    }

    public void RemovePerk(PerkType perkType)
    {
        _perks[perkType].Shutdown();
        _perks.Remove(perkType);
    }

    public bool ContainPerk(PerkType perkType)
    {
        return _perks.ContainsKey(perkType);
    }

    private void OnDestroy()
    {
        ShutdownPerks();
    }
}
