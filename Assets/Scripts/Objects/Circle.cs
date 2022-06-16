using System.Collections.Generic;
using UnityEngine;

namespace SPVD.LifeSupport
{
    public record Circle(Vector2 pos, float radius)
    {
        public List<Vector2> Points;
        public float MedianDistance;
    }
}
