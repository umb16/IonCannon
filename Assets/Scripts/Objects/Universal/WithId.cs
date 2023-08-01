using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithId
{
    private static int _counter;
    private bool _ready;
    private int _id;
    public int InstanceId
    {
        get
        {
            if (!_ready)
            {
                _id = _counter++;
                _ready = true;
            }
            return _id;
        }
    }
}
