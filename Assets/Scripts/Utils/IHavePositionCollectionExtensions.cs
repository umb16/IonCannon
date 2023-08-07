using System;
using System.Collections;
using System.Collections.Generic;
using Umb16.Extensions;
using UnityEngine;

public static class IHavePositionCollectionExtensions
{
    public static List<T> GetInRadius<T>(this IEnumerable<IHavePosition> entities, Vector3 center, float radius)
    {
        List<T> result = new List<T>();
        float sqrRadius = radius * radius;
        foreach (var entity in entities)
        {
            if (entity is not T validEntity)
                continue;

            if ((entity.Position - center).SqrMagnitudeXY() < sqrRadius)
            {
                result.Add(validEntity);
            }
        }
        return result;
    }

    public static List<T> GetInCone<T>(this IEnumerable<IHavePosition> entities, Vector3 center, float radius, Vector3 dir, float angle)
    {
        List<T> result = new List<T>();
        angle = angle / 180;
        float sqrRadius = radius * radius;
        foreach (var entity in entities)
        {
            if (entity is not T validEntity)
                continue;

            var dirToTarget = entity.Position - center;
            if ((dirToTarget).SqrMagnitudeXY() < sqrRadius && dir.DiamondAngleXY2(dirToTarget) < angle)
            {
                result.Add(validEntity);
            }
        }
        return result;
    }

    public static List<T>[] GetInCone<T>(this IEnumerable<IHavePosition> entities, Vector3 center, float radius, Vector3 dir, params float[] angles)
    {
        List<T>[] result = new List<T>[angles.Length];
        Array.Fill(result, new List<T>());

        for (int i = 0; i < angles.Length; i++)
        {
            angles[i] = angles[i] / 180;
        }

        float sqrRadius = radius * radius;
        foreach (var entity in entities)
        {
            if (entity is not T validEntity)
                continue;

            var dirToTarget = entity.Position - center;
            if ((dirToTarget).SqrMagnitudeXY() > sqrRadius)
                continue;

            var diamondAngle = dir.DiamondAngleXY2(dirToTarget);
            for (int i = 0; i < angles.Length; i++)
            {
                if (diamondAngle < angles[i])
                {
                    result[i].Add(validEntity);
                    break;
                }
            }
        }
        return result;
    }
}
