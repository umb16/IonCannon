using System.Collections;
using System.Collections.Generic;
using Umb16.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightningBuilder : MonoBehaviour
{
    class LightningTarget
    {
        public IMob Target;      
        public int LightningLvl;
        public IMob Source;
    }
    private IMob _attacker;
    List<LightningTarget> _curTargets = new();
    List<LightningTarget> _nextTargets = new();
    private float[] _lightningEffects = new float[] { 40, 20, 10 };
    List<Vector2> list = new List<Vector2>();
    private void OnDrawGizmos()
    {
        for (int i = 0; i < list.Count-1; i++)
        {
            Gizmos.DrawLine(list[i], list[i + 1]);
        }
    }

    public void Init(IMob attacker)
    {
        for (int i = 0; i < 100; i++)
        {
            list.Add(new Vector2(Random.value, Random.value)*Random.value*100);
        }
        _attacker = attacker;
        FindNewTargets(new LightningTarget() { Target = attacker, LightningLvl = 0, Source = attacker });

    }

    public void Update()
    {
        foreach (var target in _curTargets)
        {
            //_lightningPath.positionCount++;

            if (target.LightningLvl < _lightningEffects.Length - 1) { }
                FindNewTargets(target);
         
            target.Target.ReceiveDamage(new DamageMessage(_attacker, target.Target, _lightningEffects[target.LightningLvl], DamageSources.Electricity, 2f));
        }
    }

    private void FindNewTargets(LightningTarget source)
    {
        List<IMob> mobsAffectedBy = new();
        List<IMob> targets = source.Target.AllMobs;

        for (int i = 0; i < targets.Count; i++)
        {
            var mob = targets[i];
            if (mob == source.Target)
                continue;
            if ((mob.Position - source.Target.Position).SqrMagnetudeXY() < _lightningEffects[source.LightningLvl] * _lightningEffects[source.LightningLvl])
            {
                mobsAffectedBy.Add(mob);
            }
        }
        if (mobsAffectedBy.Count != 0)
        {
            IMob newTarget = mobsAffectedBy[Random.Range(0, mobsAffectedBy.Count)];
            int lvl = source.LightningLvl + 1;

            if (newTarget.ContainPerk(PerkType.FrostbiteEffect))
                lvl--;

            _nextTargets.Add(new LightningTarget() { Target = newTarget, LightningLvl = lvl, Source = source.Target });
        }
    }
}
