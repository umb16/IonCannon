using System.Collections;
using System.Collections.Generic;
using Umb16.Extensions;
using UnityEngine;
using Zenject;

public class GravityStone : MonoBehaviour
{
    private const int _radius = 10;
    private const int _speed = 5;
    private IMob _mob;
    private float _phaseTime;

    public void Init(IMob mob)
    {
        _mob = mob;
        CalculatePosition();
    }
    private void Update()
    {
        _phaseTime += Time.deltaTime * _speed;
        CalculatePosition();

    }

    private void CalculatePosition()
    {
        transform.position = _mob.Position + new Vector3(Mathf.Sin(_phaseTime), Mathf.Cos(_phaseTime)) * _radius;
    }

    private void OnTriggerEnter(Collider col)
    {
        var mob = col.gameObject.GetComponent<IDamagable>();
        if (mob != null)
        {
            OnTriggerEnter(mob);
        }
    }

    private void OnTriggerEnter(IDamagable mob)
    {      
        mob.ReceiveDamage(new DamageMessage(_mob, mob, 10, DamageSources.Unknown, .5f));
    }
}
