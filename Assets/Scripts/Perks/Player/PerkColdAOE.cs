using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Umb16.Extensions;
using Random = UnityEngine.Random;

public class PerkColdAOE : PerkCommonBase
{
    private IMob _mob;
    private IDisposable _loop;
    private int _maxAffectedBy;
    private float _radius;
    private float _timeTikInSec;
    private List<IMob> _mobs = new();

    public PerkColdAOE(int maxAffectedBy, float radius, float timeTikInSec)
    {
        _maxAffectedBy = maxAffectedBy;
        _radius = radius;
        _timeTikInSec = timeTikInSec;
        SetType(PerkType.ColdAOE);
    }

    public override void Init(IMob mob)
    {
        _mob = mob;
        _mobs = mob.AllMobs;
        _loop = UniTaskAsyncEnumerable.Timer(TimeSpan.FromSeconds(_timeTikInSec), TimeSpan.FromSeconds(_timeTikInSec)).Subscribe(Update);
    }

    public override void Shutdown()
    {
        _loop.Dispose();
    }

    private void Update(AsyncUnit obj)
    {
        List<IMob> mobsAffectedBy = new();

        for (int i = 0; i < _mobs.Count; i++)
        {
            var mob = _mobs[i];
            if (mob == _mob)
                continue;
            if ((mob.Position - _mob.Position).SqrMagnetudeXY() < _radius * _radius)
            {
                mobsAffectedBy.Add(mob);
            }
        }

        for (int i = 0; i < _maxAffectedBy; i++)
        {
            if (mobsAffectedBy.Count != 0)
            {
                var randomMob = mobsAffectedBy[Random.Range(0, mobsAffectedBy.Count)];
                randomMob.AddPerk(new PerkFrostbiteEffect());
                mobsAffectedBy.Remove(randomMob);
            }          
        }
    }
}
