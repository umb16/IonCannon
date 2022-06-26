using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static void Set2DPos(this Transform t, float x, float y)
    {
        t.position = new Vector3(x, y, y * Constants.ZShiftFactor);
    }
    public static void To2DPos(this Transform t, float shift = 0)
    {
        t.position = new Vector3(t.position.x, t.position.y, t.position.y * Constants.ZShiftFactor + shift);
    }
}
