using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathMethods
{
    public static Vector2 GetRandomPointInCircle(Vector2 center, float maxR, float minR = 0)
    {
        float u = Random.value + Random.value;
        float radius = Mathf.Lerp(minR, maxR, u > 1 ? 2 - u : u);
        var theta = Random.value * 2 * Mathf.PI;
        return new Vector2(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta));
    }
}
