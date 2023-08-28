using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpark : MonoBehaviour
{
    private IMob _mob;
    private float _phaseTime;
    private float _speed;
    private int _damage;
    private float _stunTime;
    private PerkType _perkType;

    public void Init()
    {      
        CalculatePosition();
    }
    private void Update()
    {
        _phaseTime += Time.deltaTime * (_speed * (_perkType == PerkType.Xenomineral ? 1f + 0.3f * _mob.Player.MineralEffectBoost : 1f));
        CalculatePosition();

    }
    public void SetParams(int damage, float phaseTime, IMob mob)
    {
        _damage = damage;
        _phaseTime = phaseTime;
        _mob = mob;
    }
    private void CalculatePosition()
    {
        transform.position = _mob.Position + new Vector3(Mathf.Sin(_phaseTime), Mathf.Cos(_phaseTime)) * 5;
    }

    private void OnTriggerEnter(Collider col)
    {
        var mob = col.gameObject.GetComponent<IDamageable>();
        if (mob != null)
        {
            OnTriggerEnter(mob);
        }
    }

    private void OnTriggerEnter(IDamageable mob)
    {
        mob.ReceiveDamage(new DamageMessage(_mob, mob, _damage * (_perkType == PerkType.Xenomineral ? (1 + _mob.Player.MineralEffectBoost) : 1), DamageSources.Physical, _stunTime));
    }
    public void SetPhaseTime(float phaseTime)
    {
        _phaseTime = phaseTime;
    }
}
