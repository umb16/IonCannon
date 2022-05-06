using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Umb16.Extensions;
using Random = UnityEngine.Random;
namespace IonCannon.Tiles
{
    public class TileMapGenerator : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private Tile[] _fullTiles;
        [SerializeField] private Tile[] _threeSideTiles;
        [SerializeField] private Tile[] _twoSideTiles;
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
        [SerializeField] private float perlinThreshold = .5f;
        private float[,] _perlinMap;
        private (Vector3Int vector, Directions enm)[] _directions =
            {
        (new Vector3Int(0, 1), Directions.Up),
        (new Vector3Int(1, 0), Directions.Right),
        (new Vector3Int(0, -1), Directions.Down),
        (new Vector3Int(-1, 0),Directions.Left)
    };
        //[SerializeField] private TilePallet _tilemap;
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
            chunk.CreateLayer(.4f, TileType.Standart);
            chunk.CreateLayer(.55f, TileType.Standart2);
            chunk.CreateLayer(.7f, TileType.Standart3);
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
            // GenerateLayer(.6f);
            // GenerateLayer(.7f);
        }

        private void SetEmpty(int i, int j, Directions dir, TileType type)
        {
            var newTile = new GLTile(8, _fullTilesCoords[Random.Range(0, _fullTilesCoords.Length)], new Vector3(i * 2, j * 2), 2, dir);
            TileDrawer._tiles[type].Add(newTile);           
            // _tilemap.SetTile(new Vector3Int(i, j), _fullTiles[Random.Range(0, _fullTiles.Length)]);
        }

        private void SetThreeSides(int i, int j, Directions dir, TileType type)
        {
            var newTile = new GLTile(8, _threeSideTilesCoords[Random.Range(0, _threeSideTilesCoords.Length)], new Vector3(i * 2, j * 2), 2, dir);

            TileDrawer._tiles[type].Add(newTile);

            /* Quaternion quaternion;
             switch (dir)
             {
                 case Directions.Right:
                     quaternion = Quaternion.Euler(0, 0, -90);
                     break;
                 case Directions.Down:
                     quaternion = Quaternion.Euler(0, 0, 180);
                     break;
                 case Directions.Left:
                     quaternion = Quaternion.Euler(0, 0, 90);
                     break;
                 default:
                     quaternion = Quaternion.identity;
                     break;
             }
             Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, quaternion, new Vector3(Random.value < .5f ? 1 : -1, 1, 1));
             var tileCD = new TileChangeData(new Vector3Int(i, j),
                 _threeSideTiles[Random.Range(0, _threeSideTiles.Length)],
                 Color.white, matrix);
             _tilemap.SetTile(tileCD, true);*/
        }

        private void SetTwoSides(int i, int j, Directions dir, TileType type)
        {
            var newTile = new GLTile(8, _twoSideTilesCoords[Random.Range(0, _twoSideTilesCoords.Length)], new Vector3(i * 2, j * 2), 2, dir);
            
            TileDrawer._tiles[type].Add(newTile);
            /*Matrix4x4 matrix; //= Matrix4x4.TRS(Vector3.zero, quaternion, new Vector3(Random.value < .5f ? 1 : -1, 1, 1));
            switch (dir)
            {
                case Directions.RD:
                    if (Random.value < .5f)
                        matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, -90), Vector3.one);
                    else
                        matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, -1, 1));
                    break;
                case Directions.DL:
                    if (Random.value < .5f)
                        matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, 180), Vector3.one);
                    else
                        matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(-1, -1, 1));
                    break;
                case Directions.LU:
                    if (Random.value < .5f)
                        matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, 90), Vector3.one);
                    else
                        matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(-1, 1, 1));
                    break;
                default:
                    matrix = Matrix4x4.identity;
                    break;
            }
            var tileCD = new TileChangeData(new Vector3Int(i, j),
                _twoSideTiles[Random.Range(0, _twoSideTiles.Length)],
                Color.white, matrix);
            _tilemap.SetTile(tileCD, true);*/
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
