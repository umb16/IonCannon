using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkGravityStone : WithId, IPerk
{
    private IMob _mob;
    private GravityStone _stone;

    public bool IsCommon => true;

    public PerkType Type => PerkType.GravityStone;

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
        //_loops.Add(UniTaskAsyncEnumerable.EveryValueChanged(this, x => x.SpawnTimeCome)
    }

    public void Shutdown()
    {
        GameObject.Destroy(_stone);
    }

    private async UniTask CreateGravityStone()
    {
         var go = await PrefabCreator.Instantiate(Addresses.Obj_GravityStone, Vector3.zero);
        _stone = go.GetComponent<GravityStone>();
        _stone.Init(_mob);
    }
}
