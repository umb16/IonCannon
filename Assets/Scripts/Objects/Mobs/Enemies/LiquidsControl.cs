using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Umb16.Extensions;
using UnityEngine;
using Random = System.Random;

public class LiquidsControl : MonoBehaviour
{
    [SerializeField] private float _speedFactor = 1;
    private float _bigRadius = 8;
    private bool _calcReady = true;
    private Random _random = new Random();
    public static List<Liquid> _liquidsList = new List<Liquid>();
    public static List<Liquid> _liquidsAddBuffer = new List<Liquid>();
    public static List<Liquid> _liquidsRemoveBuffer = new List<Liquid>();

    public static void Add(Liquid liquid)
    {
        _liquidsAddBuffer.Add(liquid);
    }
    public static void Remove(Liquid liquid)
    {
        _liquidsRemoveBuffer.Add(liquid);
    }

    public void ApplyBuffers()
    {
        if (_liquidsAddBuffer.Count > 0)
        {
            _liquidsList.AddRange(_liquidsAddBuffer);
            _liquidsAddBuffer.Clear();
        }
        if (_liquidsRemoveBuffer.Count > 0)
        {
            foreach (Liquid toremove in _liquidsRemoveBuffer)
                _liquidsList.Remove(toremove);
            _liquidsRemoveBuffer.Clear();
        }
    }

    private void Start()
    {
        UniTaskAsyncEnumerable.EveryUpdate().Subscribe(_ =>
        {
            foreach (var liquid1 in _liquidsList)
            {
                liquid1.Move();
            }
        });
        UniTaskAsyncEnumerable.Timer(TimeSpan.FromSeconds(0.0), TimeSpan.FromSeconds(1f)).Subscribe(_ =>
        {
            if (!_calcReady)
                return;
            ApplyBuffers();
            _calcReady = false;
            foreach (var liquid1 in _liquidsList)
            {
                liquid1.BakePosition();
            }
            UniTask.RunOnThreadPool(CalcLiquids).ContinueWith(() =>
            {
                foreach (var liquid1 in _liquidsList)
                {
                    liquid1?.UnbakeDirection();
                }
                _calcReady = true;
            }).Forget();
        });
    }

    private void CalcLiquids()
    {

        foreach (var liquid1 in _liquidsList)
        {
            if(liquid1 == null)
                continue;
            Vector2 gravity = Vector2.zero;
            Vector2 repulsion = Vector2.zero;
            float countG = 0;
            float countR = 0;
            foreach (var liquid2 in _liquidsList)
            {
                if (liquid2 == null)
                    continue;
                if (liquid2.ID != liquid1.ID)
                {
                    Vector2 vector = (liquid2.BackedPosition - liquid1.BackedPosition);
                    var sqrDistance = vector.sqrMagnitude;
                    if (sqrDistance == 0)
                    {
                        vector = new Vector2((float)_random.NextDouble(), (float)_random.NextDouble());
                        sqrDistance = .1f;
                    }
                    if (sqrDistance < _bigRadius * _bigRadius)
                    {
                        if (sqrDistance < liquid1._repulsionCurrentRadius * liquid1._repulsionCurrentRadius)
                        {
                            float k = sqrDistance / (liquid1._repulsionCurrentRadius * liquid1._repulsionCurrentRadius);
                            countR++;
                            repulsion += vector * -1 / sqrDistance * (1 - k);
                        }
                        else
                        {
                           // countG++;
                           // gravity += vector / sqrDistance * liquid1._gravityK;
                        }
                    }
                }
            }
            liquid1._repulsionCurrentRadius = Mathf.Lerp(liquid1._repulsionMinRadius, liquid1._repulsionMaxRadius, countR * .333f);
            if (countR > 0)
                gravity /= countR;

            //if (!((Vector3)(gravity + repulsion)).EqualsWithThreshold(Vector3.zero, .1f))
                liquid1.BackedTarget = (gravity + repulsion);
            //else
             //   liquid1.BackedTarget = liquid1.BackedPosition;
            /* else
                 liquid1.BackedTarget = Vector2.zero;*/
                //    transform.position += ((Vector3)(gravity + repulsion)) * Time.deltaTime;
        }
    }
}
