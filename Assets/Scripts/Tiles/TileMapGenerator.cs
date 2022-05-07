using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Umb16.Extensions;
using Random = UnityEngine.Random;
namespace IonCannon.Tiles
{
    public class TileMapGenerator : MonoBehaviour
    {
        private Vector2Int[] _fullTilesCoords = { Vector2Int.zero };
        private Vector2Int[] _threeSideTilesCoords =
            {
            new Vector2Int(1,0),
            new Vector2Int(2,0),
            new Vector2Int(3,0),
            new Vector2Int(4,0),
            new Vector2Int(5,0),
            new Vector2Int(6,0),
            new Vector2Int(7,0),
            new Vector2Int(0,1),
            new Vector2Int(2,1),
            new Vector2Int(3,1),
            new Vector2Int(4,1),
            new Vector2Int(5,1),
            new Vector2Int(6,1),
        };
        private Vector2Int[] _twoSideTilesCoords =
            {
            new Vector2Int(7,1),
            new Vector2Int(0,2),
            new Vector2Int(1,2),
            new Vector2Int(2,2),
            new Vector2Int(3,2),
            new Vector2Int(4,2),
            new Vector2Int(5,2),
            new Vector2Int(6,2),
            new Vector2Int(7,2),
            new Vector2Int(0,3),
            new Vector2Int(1,3),
        };
        private int _mapSize = 100;
        [SerializeField] private float perlinFactor = .1f;
        [SerializeField] private float perlinThreshold1 = .4f;
        [SerializeField] private float perlinThreshold2 = .55f;
        [SerializeField] private float perlinThreshold3 = .7f;
        private float[,] _perlinMap;
        private (Vector3Int vector, Directions enm)[] _directions =
            {
        (new Vector3Int(0, 1), Directions.Up),
        (new Vector3Int(1, 0), Directions.Right),
        (new Vector3Int(0, -1), Directions.Down),
        (new Vector3Int(-1, 0),Directions.Left)
    };

        [EditorButton]
        private void Start()
        {
            foreach (var item in TileDrawer._tiles)
            {
                item.Value.Clear();
            }
            int xRandom = (int)(Random.value * 100);
            int yRandom = (int)(Random.value * 100);
            var chunk = new Chunk(_mapSize, xRandom, yRandom, perlinFactor);
            chunk.CreateLayer(perlinThreshold1, TileType.Standart);
            chunk.CreateLayer(perlinThreshold2, TileType.Standart2);
            chunk.CreateLayer(perlinThreshold3, TileType.Standart3);
            for (int i = 0; i < _mapSize; i++)
            {
                for (int j = 0; j < _mapSize; j++)
                {

                    var (type, directions) = chunk.GetTile(new Vector2Int(i, j));
                    if (directions == Directions.UR
                        || directions == Directions.RD
                        || directions == Directions.DL
                        || directions == Directions.LU)
                    {
                        SetTwoSides(i - _mapSize / 2, j - _mapSize / 2, directions, type);
                    }
                    else if (directions == Directions.Up
                        || directions == Directions.Right
                        || directions == Directions.Down
                        || directions == Directions.Left)
                    {
                        SetThreeSides(i - _mapSize / 2, j - _mapSize / 2, directions, type);
                    }
                    else if (directions == Directions.None)
                        SetEmpty(i - _mapSize / 2, j - _mapSize / 2, directions, type);

                }
            }
        }

        private void SetEmpty(int i, int j, Directions dir, TileType type)
        {
            var newTile = new GLTile(8, _fullTilesCoords[Random.Range(0, _fullTilesCoords.Length)], new Vector3(i * 2, j * 2), 2, dir);
            TileDrawer._tiles[type].Add(newTile);           
        }

        private void SetThreeSides(int i, int j, Directions dir, TileType type)
        {
            var newTile = new GLTile(8, _threeSideTilesCoords[Random.Range(0, _threeSideTilesCoords.Length)], new Vector3(i * 2, j * 2), 2, dir);

            TileDrawer._tiles[type].Add(newTile);
        }

        private void SetTwoSides(int i, int j, Directions dir, TileType type)
        {
            var newTile = new GLTile(8, _twoSideTilesCoords[Random.Range(0, _twoSideTilesCoords.Length)], new Vector3(i * 2, j * 2), 2, dir);
            TileDrawer._tiles[type].Add(newTile);
        }

        private (int count, Directions direction) GetNormalSidesCount(Vector3Int position, float threshold)
        {
            int result = 0;
            Directions dir = Directions.None;
            for (int i = 0; i < _directions.Length; i++)
            {
                Vector3Int direction = _directions[i].vector;
                Vector3Int currentPosition = position + direction;
                if (currentPosition.x < 0)
                    currentPosition.x = _perlinMap.GetLength(0) - 1;
                if (currentPosition.x >= _perlinMap.GetLength(0))
                    currentPosition.x = 0;
                if (currentPosition.y >= _perlinMap.GetLength(1))
                    currentPosition.y = 0;
                if (currentPosition.y < 0)
                    currentPosition.y = _perlinMap.GetLength(1) - 1;
                if (_perlinMap[currentPosition.x, currentPosition.y] > threshold)
                    result++;
                else
                    dir |= _directions[i].enm;
            }

            return (result, dir);
        }
    }
}
