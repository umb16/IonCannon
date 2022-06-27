using System.Collections.Generic;
using UnityEngine;

namespace SPVD.LifeSupport
{
    public record Circle(Vector2 pos, float radius, int ID)
    {
        public List<Vector2> Points;
        public float MedianDistance;
    }
}
