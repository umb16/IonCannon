using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarDust : MonoBehaviour
{
    private float _lifetime;

    public  void SetParams(int radius, float speed, int damage, float stunTime, float lifetime, PerkType perkType)
    {
        _radius = radius;
        _speed = speed;
        _damage = damage;
        _stunTime = stunTime;
        _lifetime = lifetime;
        _perkType = perkType;
    }
}
