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
    Random rand = new Random(234234);
    [BurstCompile]
    struct CalcLiquidForcesJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<float2> Positions;
        [WriteOnly] public NativeArray<float2> Results;
        public float _GRadius;
        public float _RRadius;
        public float _RRadiusMin;
        public float _RRadiusMax;
        public Random rand;
        public void Execute(int i)
        {
            //Random rand = new Random(234234);
            //for (int i = 0; i < Positions.Length; i++)
            {
                if (Positions[i].x == float.PositiveInfinity&& Positions[i].y == float.PositiveInfinity)
                {
                    Results[i] = Positions[i];
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
                        var sqrDistance = math.distancesq(secondPos, currentPos);
                        if (sqrDistance == 0)
                        {
                            vector = rand.NextFloat2() + new float2(-.5f,-.5f);
                            sqrDistance = _RRadius * _RRadius-1f;
                            //continue;
                        }
                        //var r = math.lerp(_RRadiusMin, _RRadiusMax, countR * .333f);
                        if (sqrDistance < _GRadius * _GRadius)
                        {
                            if (sqrDistance < _RRadius * _RRadius)
                            {
                                float k = sqrDistance / (_RRadius * _RRadius);
                                countR++;
                                repulsion += vector * -1 / sqrDistance * (1 - k);
                            }
                            else
                            {
                                countG++;
                                gravity += vector / sqrDistance * 1;
                            }
                        }
                    }


                }
                if (countR > 0)
                    gravity /= countR;
                if(math.distancesq(Positions[i], Positions[i] + (gravity + repulsion))>.01f)
                Results[i] = Positions[i] + (gravity + repulsion);

            }
            

            /* for (int i = 0; i < A.Length; ++i)
             {
                 C[i] = A[i] + B[i];
             }*/
        }
    }
    // Start is called before the first frame update
    CalcLiquidForcesJob xxxx;
    const int size = 1000;
    const Allocator alloc = Allocator.Persistent;
    NativeArray<float2> x;
    NativeArray<float2> res;
    void Start()
    {
        x = new NativeArray<float2>(size, alloc);
        res = new NativeArray<float2>(size, alloc);
        for (int i = 0; i < x.Length; i++)
         {
            //x[i] = rand.NextFloat2(-50, 50);
            x[i] = new float2(float.PositiveInfinity, float.PositiveInfinity);
         }



        xxxx = new CalcLiquidForcesJob() { Positions = x, Results = res, _GRadius = 8, _RRadius = 4, rand = rand, _RRadiusMax = 6, _RRadiusMin = 3};
        var stop = new Stopwatch();
        stop.Start();
        xxxx.Schedule(size,64).Complete();
        Debug.Log("swer " + stop.ElapsedMilliseconds);
        //x.Dispose();
        // res.Dispose();
        /*x.Dispose();
        res.Dispose();*/
    }

    private void OnDestroy()
    {
        x.Dispose();
        res.Dispose();
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
            Debug.DrawLine((Vector2)xxxx.Results[i]- Vector2.up, (Vector2)xxxx.Results[i]+Vector2.up, Color.red);
            Debug.DrawLine((Vector2)xxxx.Results[i]- Vector2.left, (Vector2)xxxx.Results[i]+Vector2.left, Color.red);
            Debug.DrawLine((Vector2)xxxx.Positions[i], (Vector2)xxxx.Results[i], Color.white, 1);
            xxxx.Positions[i] = xxxx.Results[i];
        }
        index++;

        xxxx.Positions[index% size] = rand.NextFloat2(-50, 50);
    }
}
