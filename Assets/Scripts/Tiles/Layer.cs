using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace IonCannon.Tiles
{
    public class Layer
    {
        public float Threshold { get; private set; }
        public TileType TileType { get; private set; }

        private static (Vector2Int vector, Directions enm)[] _directions =
            {
        (new Vector2Int(0, 1),Directions.Up),
        (new Vector2Int(1, 0), Directions.Right),
        (new Vector2Int(0, -1), Directions.Down),
        (new Vector2Int(-1, 0),Directions.Left)
    };
        public Dictionary<Vector2Int, Directions> ValidTiles;
        public Layer(Chunk chunk, float threshold, TileType tileType)
        {
            TileType = tileType;
            Threshold = threshold;
            Dictionary<Vector2Int, Directions> currentStatus = new Dictionary<Vector2Int, Directions>();
            List<Vector2Int> needToCheck = new List<Vector2Int>();
            needToCheck.Add(Vector2Int.zero);
            int index = 0;
            while (needToCheck.Count > 0)
            {
                index++;
                var position = needToCheck[0];
                // Debug.Log("XXX " + position);
                // Debug.Log("XXX");
                needToCheck.RemoveAt(0);
                var directions = GetDirections(position, chunk, threshold, currentStatus);
                bool currentPosStatus = GetCurrentStatus(chunk, threshold, currentStatus, position.x, position.y);
                bool currentPosValidity = false;
                if (currentPosStatus)
                    currentPosValidity = CheckValidity(directions);
                foreach (var item in GetNeighboursPos(position, chunk.Size))
                {
                    if ((currentPosStatus != currentPosValidity || !currentStatus.ContainsKey(item)) && !needToCheck.Contains(item))
                        needToCheck.Add(item);
                }
                if (currentPosValidity)
                    currentStatus[position] = directions;
                else
                    currentStatus[position] = Directions.ALL;
            }
            ValidTiles = currentStatus.Where(x => CheckValidity(x.Value)).ToDictionary(x => x.Key, x => x.Value);
        }

        private List<Vector2Int> GetNeighboursPos(Vector2Int position, int size)
        {
            List<Vector2Int> result = new List<Vector2Int>();
            for (int i = 0; i < _directions.Length; i++)
            {
                var currentPos = _directions[i].vector + position;
                if (currentPos.x < 0 || currentPos.y < 0 || currentPos.x >= size || currentPos.y >= size)
                    continue;
                result.Add(currentPos);
            }
            return result;
        }

        private static bool GetCurrentStatus(Chunk chunk, float threshold, Dictionary<Vector2Int, Directions> currentStatus, int i, int j)
        {
            if (currentStatus.TryGetValue(new Vector2Int(i, j), out var value))
            {
                return CheckValidity(value);
            }
            else
            {
                return chunk.GetСachedPerlin(i, j) > threshold;
            }
        }
        private Directions GetDirections(Vector2Int position, Chunk chunk, float threshold, Dictionary<Vector2Int, Directions> currentStatus)
        {
            Directions dir = Directions.None;
            for (int i = 0; i < _directions.Length; i++)
            {
                Vector2Int direction = _directions[i].vector;
                Vector2Int currentPosition = position + direction;
                if (!GetCurrentStatus(chunk, threshold, currentStatus, currentPosition.x, currentPosition.y))
                    dir |= _directions[i].enm;
            }
            return dir;
        }
        private static bool CheckValidity(Directions directions)
        {
            return directions != Directions.ALL
                    && directions != Directions.UD
                    && directions != Directions.LR
                    && directions != Directions.URD
                    && directions != Directions.RDL
                    && directions != Directions.DLU
                    && directions != Directions.LUR;
        }
    }
}
