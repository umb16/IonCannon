using System.Collections;
using System.Collections.Generic;
using Umb16.Extensions;
using UnityEngine;
using Zenject;

public class OrbitalProjectile : MonoBehaviour
{
    private IMob _mob;
    private float _phaseTime;
    private int _radius;
    private float _speed;
    private int _damage;
    private float _stunTime;
    private PerkType _perkType;
    public float Lifetime;

    public void Init(IMob mob)
    {
        _mob = mob;

        CalculatePosition();
    }
    private void Update()
    {

        _phaseTime += Time.deltaTime * (_speed * (_perkType == PerkType.Xenomineral? 1f + 0.3f *_mob.Player.MineralEffectBoost : 1f));  
        CalculatePosition();

    }
    public void SetParams(int radius, float speed, int damage, float stunTime, PerkType perkType)
    {
        _radius = radius;
        _speed = speed;
        _damage = damage;
        _stunTime = stunTime;
        _perkType = perkType;
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
        mob.ReceiveDamage(new DamageMessage(_mob, mob, _damage * (_perkType == PerkType.Xenomineral ? (1 + _mob.Player.MineralEffectBoost) : 1), DamageSources.Physical, _stunTime));
    }
    public void SetPhaseTime(float phaseTime)
    {
        _phaseTime = phaseTime;
    }
}
