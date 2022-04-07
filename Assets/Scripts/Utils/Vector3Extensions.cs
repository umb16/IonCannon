using UnityEngine;

namespace Umb16.Extensions
{
    public static class Vector3Extensions
    {

        public static Vector3 Get2D(this Vector3 v)
        {
            return new Vector3(v.x, v.y, v.y * Constants.ZShiftFactor);
        }
        public static bool EqualsWithThreshold(this Vector3 v1, Vector3 v2, float threshold)
        {
            return Mathf.Abs(v1.x - v2.x) < threshold && Mathf.Abs(v1.y - v2.y) < threshold &&
                   Mathf.Abs(v1.z - v2.z) < threshold;
        }

        public static Vector3 Abs(this Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }

        /// <summary>
        /// Возвращает число в диапазоне от 0 до 4 где:
        /// <para>0 это направление по оси х;
        /// 1 по оси z;
        /// 2 противоположно оси x;
        /// 3 противоположно оси z;</para>
        /// </summary>
        ///  <returns></returns>
        public static float DiamondAngleXZ4(this Vector3 v)
        {
            if (v.z >= 0)
                return v.x >= 0 ? v.z / (v.x + v.z) : 1 - v.x / (-v.x + v.z);
            return v.x < 0 ? 2 - v.z / (-v.x - v.z) : 3 + v.x / (v.x - v.z);
        }

        public static float DiamondAngleXZ4(this Vector3 v1, Vector3 v2)
        {
            var a = v1.DiamondAngleXZ4();
            var b = v2.DiamondAngleXZ4();
            float angle = b - a;
            if (angle < 0)
                angle += 4;
            return angle;
        }

        public static float DiamondAngleXZSign2(this Vector3 v1, Vector3 v2)
        {
            float angle = DiamondAngleXZ4(v1, v2);
            if (angle > 2)
                angle = -(4 - angle);
            return angle;
        }

        /// <summary>
        /// Угол между векторами в диапозоне от 0 до 2
        /// </summary>
        public static float DiamondAngleXZ2(this Vector3 v1, Vector3 v2)
        {
            var a = v1.DiamondAngleXZ4();
            var b = v2.DiamondAngleXZ4();

            float angle;
            if (a < b)
            {
                angle = b - a;
            }
            else
            {
                angle = a - b;
            }

            if (angle > 2)
            {
                angle = 4 - angle;
            }

            return angle;
        }

        public static float SqrMagnetudeXZ(this Vector3 v)
        {
            return v.x * v.x + v.z * v.z;
        }

        public static float DotXY(this Vector3 v1, Vector3 v2)
        {
            return v1.x * v2.x + v1.y * v2.y;
        }

        public static Vector3 RotateXY(this Vector3 v, Vector3 rotator)
        {
            v = new Vector3(rotator.x, rotator.y, v.z) * v.x + new Vector3(rotator.y, -rotator.x, v.z) * v.y;
            return v;
        }

        public static Vector3 DiamondRotateXY(this Vector3 v, float angle)
        {
            return v.RotateXY(DiamondToNormalVectorXY(angle));
        }

        public static Vector3 RotateXY90(this Vector3 v)
        {
            return new Vector3(v.y, -v.x, v.z);
        }

        public static Vector3 Min(this Vector3 v1, Vector3 v2)
        {
            if (v1.x > v2.x)
            {
                v1.x = v2.x;
            }
            if (v1.y > v2.y)
            {
                v1.y = v2.y;
            }
            if (v1.z > v2.z)
            {
                v1.z = v2.z;
            }
            return v1;
        }

        public static Vector3 Max(this Vector3 v1, Vector3 v2)
        {
            if (v1.x < v2.x)
            {
                v1.x = v2.x;
            }
            if (v1.y < v2.y)
            {
                v1.y = v2.y;
            }
            if (v1.z < v2.z)
            {
                v1.z = v2.z;
            }
            return v1;
        }

        public static Vector3 DiamondToVectorXY(float angle)
        {
            float x = (angle < 2 ? 1 - angle : angle - 3);
            float y = (angle < 3 ? ((angle > 1) ? 2 - angle : angle) : angle - 4);
            return new Vector3(x, y, 0);
        }
        public static Vector3 DiamondToNormalVectorXY(float angle)
        {
            return DiamondToVectorXY(angle).NormalizedXY();
        }
        public static Vector3 NormalizedXY(this Vector3 v)
        {
            var x = Mathf.Sqrt(v.x * v.x + v.y * v.y);
            v.x /= x;
            v.y /= x;
            return v;
        }

        public static float SqrMagnetudeXY(this Vector3 v)
        {
            return v.x * v.x + v.y * v.y;
        }

        public static float MagnetudeXY(this Vector3 v)
        {
            return Mathf.Sqrt(v.x * v.x + v.y * v.y);
        }
    }
}
