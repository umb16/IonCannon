using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class PerkStarDust : PerkGravityMatter
{
    private List<GravityStone> _dust = new();
    private List<float> _timers = new ();

    public PerkStarDust(int radius, float speed, int damage, float stunTime, float size, PerkType perkType) : base(radius, speed, damage, stunTime, size, perkType)
    {

    }
    public override void Add(IPerk perk)
    {

    }
    public override void Init(IMob mob)
    {
        _mob = mob;
        _mob.PickedUpScoreGem += OnPickingUpScoreGem;
    }
    public void OnPickingUpScoreGem()
    {

        CreateGravityStone().Forget();
    }
    public override async UniTask CreateGravityStone()
    {
        var go = await PrefabCreator.Instantiate(Addresses.Obj_GravityStone, Vector3.zero);
        GravityStone _newStone = go.GetComponent<GravityStone>();
        _newStone.SetParams(_radius, _speed, _damage, _stunTime, _perkType);
        _newStone.Init(_mob);
        _dust.Add(_newStone);
        _timers.Add(5);
    }
    public override void Shutdown()
    {
        _mob.PickedUpScoreGem -= OnPickingUpScoreGem;

        foreach (var item in _dust)
        {
            GameObject.Destroy(item?.gameObject);
        }
        _dust.Clear();
    }
    private void Update()
    {
        for (int i = 0; i < _timers.Count; i++)
        {
            _timers[i] -= Time.deltaTime;
            if (_timers[i] <= 0)
            {               
                GameObject.Destroy(_dust[i].gameObject);
                _dust.RemoveAt(i);
                _timers.RemoveAt(i);
                i--;
            }
        }
    }
}
