using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkGravityMatter : WithId, IPerk
{
    private IMob _mob;
    private GravityStone _stone;
    private int _radius;
    private float _speed;
    private int _damage;
    private float _stunTime;
    private PerkType _perkType;

    public PerkGravityMatter(int radius, float speed, int damage, float stunTime, PerkType perkType)
    {
        _radius = radius;
        _speed = speed;
        _damage = damage;
        _stunTime = stunTime;
        _perkType = perkType;
    }

    public bool IsCommon => true;

    public PerkType Type => _perkType;

    public void Add(IPerk perk)
    {
        
    }

    public string GetDescription()
    {
        return "";
    }

    public void Init(IMob mob)
    {
        _mob = mob;
        CreateGravityStone().Forget();
    }

    public void Shutdown()
    {
        GameObject.Destroy(_stone);
    }

    private async UniTask CreateGravityStone()
    {
         var go = await PrefabCreator.Instantiate(Addresses.Obj_GravityStone, Vector3.zero);
        _stone = go.GetComponent<GravityStone>();
        _stone.SetParams(_radius, _speed, _damage, _stunTime, _perkType);
        _stone.Init(_mob);
    }
}
