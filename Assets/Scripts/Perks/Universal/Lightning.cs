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
    private LightningTarget _lightSource;
    private List<LightningTarget> _nextTargets = new();
    private List<IMob> _formerTargets = new();
    private float[] _lightningRadius = new float[] { 25, 15, 15 };
    private float[] _lightningDamage = new float[] { 40, 20, 10 };
    List<LightningSegment> _lightningSegments = new();

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _lightningSegments.Count; i++)
        {
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
        if (_nextSegmentTime <= Time.time)
        {
            if (_nextTargets.Count == 0)
            {
                Debug.Log("Destroy");
                Destroy(this.gameObject);
                return;
            }

            List<LightningTarget> _curTargets = new(_nextTargets);
            _nextTargets = new();

            foreach (var target in _curTargets)
            {
                if (target.LightningLvl < _lightningRadius.Length - 1)
                    FindNewTargets(target);

                _lightningSegments.Add(new LightningSegment()
                {
                    StartPoint = target.Source.Position,
                    FinishPoint = target.Mob.Position,
                    TimeToDisappear = Time.time + 0.5f
                });

                target.Mob.ReceiveDamage(new DamageMessage(_attacker, target.Mob, _lightningDamage[target.LightningLvl], DamageSources.Electricity, 2f));
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
        _lightSource = source;

        float radius = _lightningRadius[_lightSource.LightningLvl];
        Vector3 sourcePos = source.Mob.Position;
        List<IMob> mobsAffectedBy = new(_attacker.AllMobs.Where<IMob>(x => (x != _attacker) || (x != source.Mob)));

        if (source.Mob == source.Source)//первый посчет отрезка молнии от кастера к первой жертве.
        {
            mobsAffectedBy = mobsAffectedBy.GetInRadius<IMob>(sourcePos, radius);
            IMob newTarget = FindTheNearestTarget(sourcePos, radius, mobsAffectedBy);
            
            if (newTarget != null)
            {
                _formerTargets.Add(newTarget);

                _nextTargets.Add(new LightningTarget()
                {
                    Mob = newTarget,
                    LightningLvl = 0,
                    Source = source.Mob,
                    SourceDirection = newTarget.Position - sourcePos
                });
            }
        }
        else
        {
            FindNextTargets(radius, sourcePos, mobsAffectedBy);
        }
    }
    private void FindNextTargets(float radius, Vector3 sourcePos, List<IMob> mobsAffectedBy)
    {
        int lvl = _lightSource.LightningLvl + 1;
        List<IMob>[] mobsByDirections = mobsAffectedBy.GetInCone<IMob>(sourcePos,
                                                        radius,
                                                        _lightSource.SourceDirection,
                                                        new float[] { 90, 180 });

        for (int i = 0; i < mobsByDirections.Length; i++)
        {
            bool targetFound = false;
            int targetsCount = i == 0 ? 2 : 1;

            for (int j = 0; j < targetsCount; j++)
            {
                if (mobsByDirections[i].Count == 0)
                    break;         

                    //IMob newTarget = FindTheNearestTarget(sourcePos, radius, mobsByDirections[i]);
                    IMob newTarget = mobsByDirections[i][Random.Range(0, mobsByDirections[i].Count)];

                if (newTarget != null)
                {
                    _formerTargets.Add(newTarget);
                    mobsByDirections[i].Remove(newTarget);
                    targetFound = true;
                    if (newTarget.ContainPerk(PerkType.FrostbiteEffect))
                        lvl--;

                    _nextTargets.Add(new LightningTarget()
                    {
                        Mob = newTarget,
                        LightningLvl = lvl,
                        Source = _lightSource.Mob,
                        SourceDirection = newTarget.Position - sourcePos
                    });
                }
            }

            if (targetFound)
                break;

        }
    }

    private IMob FindTheNearestTarget(Vector2 center, float bigestRadius, List<IMob> mobs)
    {
        IMob nearest = null;

        if (mobs?.Count != 0)
        {
            float distance = bigestRadius;

            for (int i = 0; i < mobs.Count; i++)
            {
                float iDistance = Vector2.Distance(center, mobs[i].Position);

                if (iDistance < distance && !_formerTargets.Contains(mobs[i]))
                {
                    nearest = mobs[i];
                    distance = iDistance;
                }
            }
        }

        return nearest;
    }
}
