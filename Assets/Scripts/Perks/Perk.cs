using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePerk
{
    public int Level { get; private set; }
    public bool Maxed => Level == _maxLevel;
    private int _maxLevel;

}
