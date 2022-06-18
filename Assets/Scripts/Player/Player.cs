using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player : Mob
{
    public ComplexStat Gold { get; private set; } = new ComplexStat(0);
    private ComplexStat _maxPathLength;
    private ComplexStat _raySpeed;
    private ComplexStat _rayDelay;
    private ComplexStat _raySplashRadius;
    private ComplexStat _rayDamage;

    public GameObject Blood;
    private ComplexStat _lifeSupport;
    public Inventory Stash = new Inventory();

    public float RayDmg => _rayDamage.Value;

    public float RayDelay => _rayDelay.Value;

    public float RaySplash => _raySplashRadius.Value;

    public float RaySpeed => _raySpeed.Value;

    public float MaxPathLength => _maxPathLength.Value;

    [Inject]
    private void Construct(DamageController damageController, AsyncReactiveProperty<Player> player)
    {
        StatsCollection = StatsCollectionsDB.StandartPlayer();
        _rayDamage = StatsCollection.GetStat(StatType.RayDamage);
        _maxPathLength = StatsCollection.GetStat(StatType.RayPathLenght);
        _raySpeed = StatsCollection.GetStat(StatType.RaySpeed);
        _rayDelay = StatsCollection.GetStat(StatType.RayDelay);
        _raySplashRadius = StatsCollection.GetStat(StatType.RayDamageAreaRadius);
        _lifeSupport = StatsCollection.GetStat(StatType.LifeSupport);
        _lifeSupport.ValueChanged += LifeSupportValueChanged;
        _stopped = false;
        player.Value = this;
    }

    protected override void Awake()
    {
        base.Awake();
        /* Inventory.Add(_itemsDB.Coprocessor());
         Inventory.Add(_itemsDB.Coprocessor());*/
        //Inventory.Add(_itemsDB.IonizationUnit());
        // Inventory.Add(_itemsDB.IonizationUnit());
        /*Inventory.Add(_itemsDB.DeliveryDevice());*/
        /*Inventory.Add(_itemsDB.DivergingLens());*/
        /*Inventory.Add(_itemsDB.IonizationUnit());
        Inventory.Add(_itemsDB.IonizationUnit());
        Inventory.Add(_itemsDB.IonizationUnit());
        Inventory.Add(_itemsDB.IonizationUnit());
        

        Stash.Add(_itemsDB.IonizationUnit());
        Stash.Add(_itemsDB.IonizationUnit());
        Stash.Add(_itemsDB.IonizationUnit());
        Stash.Add(_itemsDB.IonizationUnit());
        Stash.Add(_itemsDB.IonizationUnit());
        Stash.Add(_itemsDB.IonizationUnit());*/
    }

    protected override void Start()
    {
        base.Start();
        AddPerk(new PerkEAfterDeathExplosion() { Delay = 0 });
        // GameData.GameStateChanged += GameStateChanged;
    }

    /*private void GameStateChanged(GameState state)
    {
        if (state == GameState.Restart)
        {
            Destroy(gameObject);
        }
    }*/

    public bool AddItemDirectly(Item item)
    {
        if (Inventory.FreeSlotAvailable && !Inventory.ContainsUnique(item))
        {
            Inventory.Add(item);
            return true;
        }
        if (Stash.FreeSlotAvailable)
        {
            Stash.Add(item);
            return true;
        }
        return false;
    }

    private void LifeSupportValueChanged(ComplexStat stat)
    {
        if (stat.Value <= 0)
        {
            ReceiveDamage(new DamageMessage(this, this, 1000, DamageSources.Unknown));
        }
    }

    public override void Die(DamageMessage message)
    {
        base.Die(message);
        Stop();
    }

    private void Stop()
    {
        _spriteRenderer.enabled = false;
        //Destroy(gameObject);
        _stopped = true;
        if (Blood != null)
            Destroy(Instantiate(Blood, transform.position + Vector3.back * 0.5f, Blood.transform.rotation), 10f);
    }

    protected override void OnDestroy()
    {
        //GameData.GameStateChanged -= GameStateChanged;
        base.OnDestroy();
    }
    protected override void Update()
    {
        base.Update();
        if (_stopped)
            return;
        Movement();
    }

    private void Movement()
    {
        Vector3 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            dir += Vector3.up;
        if (Input.GetKey(KeyCode.S))
            dir += Vector3.down;

        if (Input.GetKey(KeyCode.A))
            dir += Vector3.left;
        if (Input.GetKey(KeyCode.D))
            dir += Vector3.right;
        if (dir != Vector3.zero)
        {
            _animator.SetBool("Run", true);
            _animator.speed = MovementSpeed.Value / 6;
        }
        else
        {
            _animator.speed = 1;
            _animator.SetBool("Run", false);
        }
        MoveTo(transform.position + dir * 10);
    }
}
