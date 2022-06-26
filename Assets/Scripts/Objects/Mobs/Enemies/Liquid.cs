using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Umb16.Extensions;
using System;
using Random = UnityEngine.Random;
using TMPro;

public class Liquid : Mob
{
    [SerializeField] public float _colliderRadius;
    Vector2Int[] directions1 = { new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, -1) };
    Vector2Int[] directions2 = { new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(0, -1), new Vector2Int(-1, -1), new Vector2Int(1, -1) };
    private Timer _deathTimer;
    private Vector2Int _cellPosition;
    static float _sellSize = 1.5f;
    public static Dictionary<Vector2Int, Liquid> _liquids = new Dictionary<Vector2Int, Liquid>();
    private bool _medium;
    private bool _sleep;

    public static Vector2Int ToCellPosition(Vector3 vector)
    {
        int x = Mathf.RoundToInt(vector.x / _sellSize);
        int y = Mathf.RoundToInt((vector.y - (x % 2 == 0 ? _sellSize * .5f : 0)) / _sellSize);
        return new Vector2Int(x, y);
    }
    public static Vector3 ToRealPosition(Vector2Int vector)
    {
        return new Vector3(vector.x * _sellSize, vector.y * _sellSize + (vector.x % 2 == 0 ? _sellSize * .5f : 0), 100);
    }
    public static Vector3 SnapToGrid(Vector3 vector)
    {
        int x = Mathf.RoundToInt(vector.x / _sellSize);
        int y = Mathf.RoundToInt((vector.y - (x % 2 == 0 ? _sellSize * .5f : 0)) / _sellSize);
        return new Vector3(x * _sellSize, y * _sellSize + (x % 2 == 0 ? _sellSize * .5f : 0), 100);
    }

    public bool GetRandomFreeCell(Vector2Int cellPos, out Vector2Int result)
    {
        Vector2Int? resultx = null;
        int specialIndex = 1;
        var directions = (cellPos.x % 2 == 0 ? directions1 : directions2);
        for (int i = 0; i < directions.Length; i++)
        {
            var dir = directions[i];
            var currentPos = cellPos + dir;

            if (!_liquids.ContainsKey(currentPos))
            {
                if (Random.value < 1f / specialIndex)
                {
                    resultx = currentPos;
                }
                specialIndex++;
                //Debug.DrawLine(ToRealPosition(currentPos), ToRealPosition(cellPos), Color.yellow, 5);
            }
            //else
               // Debug.DrawLine(ToRealPosition(currentPos), ToRealPosition(cellPos), Color.green, 5);
        }
        if (resultx != null)
        {
            result = resultx.Value;
            return true;
        }
        else
        {
            result = Vector2Int.zero;
            return false;
        }
    }
    public bool GetRandomAround(Vector2Int cellPos, out Liquid result)
    {
        result = null;
        int specialIndex = 1;
        var directions = (cellPos.x % 2 == 0 ? directions1 : directions2);
        for (int i = 0; i < directions.Length; i++)
        {
            var dir = directions[i];
            var currentPos = cellPos + dir;
            if (_liquids.TryGetValue(currentPos, out var liquid))
            {
                if (Random.value < 1f / specialIndex)
                {
                    result = liquid;
                }
                specialIndex++;
            }
        }
        return result != null;
    }

    internal void Push(int id)
    {
        _medium = true;
       // Debug.Log(id + " Push " + ID);
    }

    protected override void Start()
    {
        HP = new ComplexStat(10);
        StatsCollection = new StandartStatsCollection(new (StatType type, ComplexStat stat)[]
        {
            (StatType.HP,new ComplexStat(10)),
        });
        Defence = StatsCollection.GetStat(StatType.Defence);
        Vector3 vector = transform.position;
        vector.z = 100;
        transform.position = vector;
        AddPerk(new PerkAura(PerkType.ESlowAura, PerkType.ESlowAuraEffect,
            new[] { new StatModificator(-.5f, StatModificatorType.Multiplicative, StatType.MovementSpeed) },
            2, true));
        _cellPosition = ToCellPosition(transform.position);
        if (_liquids.ContainsKey(_cellPosition))
        {
            _medium = true;
            TryToMove();
        }
        else
        {
            _liquids.Add(_cellPosition, this);
            //Debug.Log("start OK " + _cellPosition);
        }
        GetComponentInChildren<TMP_Text>().text = ID.ToString();
    }

    private void TryToMove()
    {
        if (GetRandomFreeCell(_cellPosition, out var _cell))
        {
            if (!_medium)
                _liquids.Remove(_cellPosition);
            _medium = false;
            _cellPosition = _cell;
            _liquids.Add(_cellPosition, this);
            //Debug.Log(ID + " GetRandomFreeCell OK");
        }
        else
        {
            // GetAllAround(_cellPosition).ForEach(x => x.Push());
            if (GetRandomAround(_cellPosition, out var result))
            {
                _medium = false;
                _cellPosition = result._cellPosition;
                _liquids[_cellPosition] = this;
                result.Push(ID);
            }
           // else
           //     Debug.Log(ID + "nothing to push");
        }
    }

    protected override void Update()
    {
        if (_medium)
        {
            _sleep = false;
            TryToMove();
        }
        else
        {
            if (!_sleep)
            {
                var targetPos = ToRealPosition(_cellPosition);
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime);
                if ((transform.position - targetPos).SqrMagnetudeXY() < .1f)
                    _sleep = true;
            }
        }
        //Debug.DrawLine(transform.position, ToRealPosition(_cellPosition), Color.red);
    }

    public override void ReceiveDamage(DamageMessage message)
    {
        base.ReceiveDamage(message);
    }
    public override void Die(DamageMessage message)
    {
        base.Die(message);
        _deathTimer = new Timer(.5f)
            .SetUpdate(x => _spriteRenderer.color *= new Color(1, 1, 1, 1 - x))
            .SetEnd(() => Destroy(gameObject));
    }
    protected override void OnDestroy()
    {
        _deathTimer?.Stop();
        _liquids.Remove(_cellPosition);
        base.OnDestroy();
    }
}
