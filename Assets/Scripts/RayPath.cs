using System.Collections.Generic;
using UnityEngine;

public class RayPathPoint
{
    public Vector3 Position;
    public float Distance;
}

public class RayPath
{
    private readonly List<RayPathPoint> _list = new List<RayPathPoint>();
    private float _maxLenght = 1;

    private RayPathPoint LastPoint => _list[_list.Count - 1];

    /// <returns>возвращает false когда длинна пути превышена</returns>
    public bool AddPoint(Vector3 point)
    {
        Vector3 direction = (point - LastPoint.Position);
        float distance = direction.magnitude;
        if (_maxLenght < LastPoint.Distance + distance)
        {
            distance = LastPoint.Distance + distance - _maxLenght;
            point = LastPoint.Position + direction.normalized * distance;
            return false;
        }

        return true;
    }

    public Vector3 GetPoint(float distance, int startIndex = 0)
    {
        if (distance < 0)
            return _list[0].Position;

        for (int i = startIndex; i < _list.Count - 1; i++)
        {
            if (_list[i].Distance <= distance && _list[i + 1].Distance >= distance)
            {
                return _list[i].Position + (_list[i + 1].Position - _list[i].Position).normalized * (distance - _list[i].Distance);
            }
        }
        return _list[_list.Count - 1].Position;
    }
}