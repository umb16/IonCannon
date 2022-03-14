using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fx
{
    public static int IdCounter;
    public int Id;
    public string Key;
    public FxPosition FxPosition;

    public Fx(string key, FxPosition fxPosition)
    {
        Id = IdCounter++;
        Key = key;
        FxPosition = fxPosition;
    }
}
