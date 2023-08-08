using System.Collections;
using System.Collections.Generic;
using Umb16.Extensions;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class Lightning : MonoBehaviour
{
    class LightningTarget
    {
        public IMob Mob;
        public int LightningLvl;
        public IMob Source;
        public Vector2 SourceDirection;
    }
    struct LightningSegment
    {
        public Vector3 StartPoint;
        public Vector3 FinishPoint;
        public float TimeToDisappear;
    }
    private IMob _attacker;
    private float _nextSegmentTime;
    List<LightningTarget> _nextTargets = new();
    private float[] _lightningEffects = new float[] { 25, 20, 15 };
    List<LightningSegment> _lightningSegments = new();

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _lightningSegments.Count; i++)
        {
            //Debug.Log(_lightningSegments[i].StartPoint.ToString() + "  " + _lightningSegments[i].FinishPoint.ToString());
            Gizmos.DrawLine(_lightningSegments[i].StartPoint, _lightningSegments[i].FinishPoint);
        }
    }

    public void Init(IMob attacker)
    {
        _attacker = attacker;
        _nextSegmentTime = Time.time;
        FindNewTargets(new LightningTarget() { Mob = attacker, LightningLvl = 0, Source = attacker });
    }

    public void Update()
    {
        if (_nextSegmentTime <= Time.time && _nextTargets.Count != 0)
        {
            List<LightningTarget> _curTargets = new(_nextTargets);
            _nextTargets = new();

            Debug.Log("_curTargets = " + _curTargets.Count);
            foreach (var target in _curTargets)
            {
                if (target.LightningLvl < _lightningEffects.Length - 1)
                    FindNewTargets(target);
               
                _lightningSegments.Add(new LightningSegment()
                {
                    StartPoint = target.Source.Position,
                    FinishPoint = target.Mob.Position,
                    TimeToDisappear = Time.time + 0.5f
                });

                target.Mob.ReceiveDamage(new DamageMessage(_attacker, target.Mob, _lightningEffects[target.LightningLvl], DamageSources.Electricity, 2f));
            }

            _nextSegmentTime += 0.2f;
        }

        if (_lightningSegments.Count != 0)
        {
            for (int i = 0; i < _lightningSegments.Count; i++)
            {
                if (_lightningSegments[i].TimeToDisappear <= Time.time)
                {
                    _lightningSegments.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    private void FindNewTargets(LightningTarget source)
    {
        int lvl = 0;
        IMob newTarget = null;
        Vector3 sourcePos = source.Mob.Position;
        List<IMob> mobsAffectedBy = new(_attacker.AllMobs.Where<IMob>(x => (x != _attacker) || (x != source.Mob)));

        if (source.Mob == source.Source)
        {
            mobsAffectedBy = mobsAffectedBy.GetInRadius<IMob>(sourcePos, _lightningEffects[source.LightningLvl]);
            newTarget = FindTheNearestTarget(sourcePos, mobsAffectedBy);
        }
        else
        {
            lvl = source.LightningLvl + 1;
            List<IMob>[] mobsByDirections = mobsAffectedBy.GetInCone<IMob>(source.Mob.Position,
                                                            _lightningEffects[source.LightningLvl],
                                                            source.SourceDirection,
                                                            new float[] {90,180});
           
            foreach (var list in mobsByDirections)
            {
                if(list.Count != 0)
                {
                    Debug.Log("mobsByDirections = " + list.Count);
                    newTarget = FindTheNearestTarget(sourcePos, list);

                    if (newTarget.ContainPerk(PerkType.FrostbiteEffect))
                        lvl--;

                    break;
                }
            }                          
        }

        if (newTarget != null)
        {
            _nextTargets.Add(new LightningTarget()
            {
                Mob = newTarget,
                LightningLvl = lvl,
                Source = source.Mob,
                SourceDirection = newTarget.Position - sourcePos
            });
        }
    }

    private IMob FindTheNearestTarget(Vector2 center, List<IMob> mobs)
    {
        IMob nearest = null;

        if (mobs?.Count != 0)
        {
            nearest = mobs[0];
            float distance = Vector2.Distance(center, nearest.Position);

            for (int i = 1; i < mobs.Count; i++)
            {
                float iDistance = Vector2.Distance(center, mobs[i].Position);

                if (iDistance < distance)
                {
                    nearest = mobs[i];
                    distance = iDistance;
                }
            }
        }
        
        return nearest;
    }
}
