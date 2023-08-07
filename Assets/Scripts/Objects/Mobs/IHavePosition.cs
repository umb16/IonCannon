using UnityEngine;

public interface IHavePosition
{
    Vector3 Position { get; }

    void SetPosition(float x, float y);
    void SetPosition(Vector3 vector);
}