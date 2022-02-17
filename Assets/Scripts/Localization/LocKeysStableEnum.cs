using System;
using UnityEngine;

[Serializable]
public class LocKeysStableEnum : StableEnum<LocKeys>
{
    public static implicit operator LocKeysStableEnum(LocKeys value) => new LocKeysStableEnum() { _value = value };
}