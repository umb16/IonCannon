using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using Random = Unity.Mathematics.Random;

public class LiquidTest : MonoBehaviour
{
    [SerializeField] private float _RRadiusMin = 3;
    [SerializeField] private float _RRadiusMax = 6;
    [SerializeField] private float _GRadius = 8;
    [SerializeField] private int _count = 1000;
    Random rand = new Random();
    [BurstCompile/*(FloatMode = FloatMode.Fast)*/]
    public struct CalcLiquidForcesJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<float2> Positions;
        [WriteOnly] public NativeArray<float2> Results;
        public NativeArray<float> CurrentRadius;
        public NativeArray<byte> Sleep;
        public float _GRadius;
        public float _RRadiusMin;
        public float _RRadiusMax;
        public Random rand;
        public void Execute(int i)
        {
            if (Sleep[i]>0)
            {
                Sleep[i]--;
                return;
            }
            if (Positions[i].x == float.PositiveInfinity)
            {
                //Results[i] = Positions[i];
                return;
            }
            float2 gravity = float2.zero;
            float2 repulsion = float2.zero;
            float countG = 0;
            float countR = 0;
            var currentPos = Positions[i];
            for (int j = 0; j < Positions.Length; j++)
            {

                var secondPos = Positions[j];
                if (i != j)
                {
                    float2 vector = (secondPos - currentPos);
                    var sqrDistance = math.lengthsq(vector);
                    if (sqrDistance == 0)
                    {
                        vector = rand.NextFloat2() + new float2(-.5f, -.5f);
                        sqrDistance = CurrentRadius[i] * CurrentRadius[i] - 1;
                    }

                    if (sqrDistance < _GRadius * _GRadius)
                    {
                        if (sqrDistance < CurrentRadius[i] * CurrentRadius[i])
                        {
                            float k = sqrDistance / (CurrentRadius[i] * CurrentRadius[i]);
                            countR++;
                            repulsion += vector * -1 / sqrDistance * (1 - k);
                            //Sleep[j] = false;
                        }
                        else
                        {
                            countG++;
                            //gravity += vector / sqrDistance * .1f;
                        }
                    }
                }


            }
            CurrentRadius[i] = math.lerp(_RRadiusMin, _RRadiusMax, countG * .05f);
            if (countR > 0)
                gravity /= countR;
            float2 force = gravity + repulsion;
            if (math.lengthsq(force) > 1)
                force = math.normalize(force);
            if (math.lengthsq(force) < .01f)
                Sleep[i] = 10;
            Results[i] = Positions[i] + force;
             //if (!Sleep[i] && math.lengthsq(force) < .0001f)
                
            //else
            //    Results[i] = Positions[i];
        }
    }
    // Start is called before the first frame update
    public static CalcLiquidForcesJob xxxx;
    int size = 1024;
    const Allocator alloc = Allocator.Persistent;
    NativeArray<float2> x;
    NativeArray<float2> res;
    NativeArray<float> radiuses;
    NativeArray<byte> sleep;
    private void OnEnable()
    {
        size = _count;
        rand.InitState((uint)(Time.deltaTime * 100000));
        x = new NativeArray<float2>(size, alloc);
        res = new NativeArray<float2>(size, alloc);
        radiuses = new NativeArray<float>(size, alloc);
        sleep = new NativeArray<byte>(size, alloc);
        for (int i = 0; i < x.Length; i++)
        {
            //x[i] = math.normalize(rand.NextFloat2(-1, 1))* (100 - math.pow(rand.NextFloat(10),2));
            radiuses[i] = _RRadiusMin;
            //x[i] = new float2(float.PositiveInfinity, float.PositiveInfinity);
        }



        xxxx = new CalcLiquidForcesJob()
        {
            Positions = x,
            Results = res,
            CurrentRadius = radiuses,
            Sleep = sleep,
            _GRadius = _GRadius,
            rand = rand,
            _RRadiusMax = _RRadiusMax,
            _RRadiusMin = _RRadiusMin
        };
        var stop = new Stopwatch();
        stop.Start();
        xxxx.Schedule(size, 64).Complete();
        Debug.Log("swer " + stop.ElapsedMilliseconds);
    }

    private void OnDisable()
    {
        x.Dispose();
        res.Dispose();
        radiuses.Dispose();
        sleep.Dispose();
    }

    int index = 0;
    // Update is called once per frame
    void FixedUpdate()
    {
        xxxx.Schedule(size, 64).Complete();

        for (int i = 0; i < xxxx.Positions.Length; i++)
        {
            if (xxxx.Results[i].x == float.PositiveInfinity)
                continue;
            Debug.DrawLine((Vector2)xxxx.Results[i] - Vector2.up, (Vector2)xxxx.Results[i] + Vector2.up, Color.red);
            Debug.DrawLine((Vector2)xxxx.Results[i] - Vector2.left, (Vector2)xxxx.Results[i] + Vector2.left, Color.red);
            Debug.DrawLine((Vector2)xxxx.Positions[i], (Vector2)xxxx.Results[i], Color.white, 1);
            xxxx.Positions[i] = xxxx.Results[i];
        }
        //index++;
        /*if(rand.NextFloat()<.5f)
            xxxx.Positions[index % size] = rand.NextFloat2(-50, 50);
        else
            xxxx.Positions[index % size] = new float2(float.PositiveInfinity,float.PositiveInfinity);*/
    }
}
