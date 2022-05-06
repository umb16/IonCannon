using UnityEngine;
namespace IonCannon.Tiles
{
    public class GLTile
    {
        private Vector2[] _texCoords;
        private Vector3[] _coords;
        private static readonly int[] n = { 0, 1, 2, 3 };
        private static readonly int[] h = { 3, 2, 1, 0 };
        private static readonly int[] v = { 1, 0, 3, 2 };
        private static readonly int[] vh = { 2, 3, 0, 1 };
        private static readonly int[] n90 = { 3, 0, 1, 2 };
        private static readonly int[] h90 = { 2, 1, 0, 3 };
        private static readonly int[] v90 = { 0, 3, 2, 1 };
        private static readonly int[] vh90 = { 1, 2, 3, 0 };
        public GLTile(float lineCount, Vector2 texPos, Vector3 worldPos, float size, Directions directions)
        {
            float textCoordsCorrectionShift = 0.0001f;
            Vector2 tileTextureSize = new Vector2(1 / lineCount, 1 / lineCount);
            texPos.y = lineCount - texPos.y - 1;
            Vector2 zero = tileTextureSize * texPos;
            Vector2 one = zero + tileTextureSize;
            zero += Vector2.one * textCoordsCorrectionShift;
            one -= Vector2.one * textCoordsCorrectionShift;
            _texCoords = new Vector2[4];
            var tempCoords = new Vector2[4];
            tempCoords[0] = zero;
            tempCoords[1] = new Vector2(zero.x, one.y);
            tempCoords[2] = one;
            tempCoords[3] = new Vector2(one.x, zero.y);
            var order = n;

            switch (directions)
            {
                case Directions.Up:
                    if (Random.value < .5f)
                        order = h;
                    break;
                case Directions.Right:
                    order = Random.value < .5f ? v90 : n90;
                    break;
                case Directions.Down:
                    order = Random.value < .5f ? v : vh;
                    break;
                case Directions.Left:
                    order = Random.value < .5f ? vh90 : h90;
                    break;
                case Directions.UR:
                    if (Random.value < .5f)
                        order = v90;
                    break;
                case Directions.RD:
                    order = Random.value < .5f ? v : n90;
                    break;
                case Directions.DL:
                    order = Random.value < .5f ? vh : h90;
                    break;
                case Directions.LU:
                    order = Random.value < .5f ? h : vh90;
                    break;
            }

            for (int i = 0; i < order.Length; i++)
            {
                _texCoords[i] = tempCoords[order[i]];
            }
            float posCorrectionShift = 0.00f;
            Vector3 posZero = worldPos;
            Vector3 posOne = worldPos + new Vector3(size, size);
            posZero -= Vector3.one * posCorrectionShift;
            posOne += Vector3.one * posCorrectionShift;
            _coords = new Vector3[4];
            _coords[0] = posZero;
            _coords[1] = new Vector3(posZero.x, posOne.y);
            _coords[2] = posOne;
            _coords[3] = new Vector3(posOne.x, posZero.y);
        }
        public void Draw()
        {
            GL.TexCoord(_texCoords[0]);
            GL.Vertex(_coords[0]);
            GL.TexCoord(_texCoords[1]);
            GL.Vertex(_coords[1]);
            GL.TexCoord(_texCoords[2]);
            GL.Vertex(_coords[2]);
            GL.TexCoord(_texCoords[3]);
            GL.Vertex(_coords[3]);
        }
    }
}
