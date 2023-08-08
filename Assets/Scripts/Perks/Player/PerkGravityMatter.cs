using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkGravityMatter : PerkCommonBase
{
    protected IMob _mob;
    protected OrbitalProjectile _stone;
    protected int _radius;
    protected float _speed;
    protected int _damage;
    protected float _stunTime;
    protected float _size;
    protected PerkType _perkType;

    public PerkGravityMatter(int radius, float speed, int damage, float stunTime, float size, PerkType perkType)
    {
        _radius = radius;
        _speed = speed;
        _damage = damage;
        _stunTime = stunTime;
        _size = size;
        SetType(perkType);
    }

    public override void Init(IMob mob)
    {
        _mob = mob;
        CreateGravityStone().Forget();
    }

    public override void Shutdown()
    {
        if (_stone != null)
            GameObject.Destroy(_stone.gameObject);
    }

    public virtual async UniTask CreateGravityStone()
    {
        var go = await PrefabCreator.Instantiate(Addresses.Obj_GravityStone, Vector3.zero);
        _stone = go.GetComponent<OrbitalProjectile>();
        _stone.SetParams(_radius, _speed, _damage, _stunTime, _perkType);
        _stone.Init(_mob);
    }
}