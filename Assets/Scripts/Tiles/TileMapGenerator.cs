using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Umb16.Extensions;
using System;
using Random = UnityEngine.Random;

[Flags]
public enum Directions
{
    None = 0,
    Up = 1,
    Right = 2,
    Down = 4,
    Left = 8,
    UR = Up + Right,
    RD = Right + Down,
    DL = Down + Left,
    LU = Left + Up
}

public class TileMapGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private Tile[] _fullTiles;
    [SerializeField] private Tile[] _threeSideTiles;
    [SerializeField] private Tile[] _twoSideTiles;
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
        _perlinMap = new float[_mapSize, _mapSize];
        float xShift = Random.value * 100;
        float yShift = Random.value * 100;
        for (int i = 0; i < _perlinMap.GetLength(0); i++)
        {
            for (int j = 0; j < _perlinMap.GetLength(1); j++)
            {
                float perlin = Mathf.PerlinNoise(i * perlinFactor + xShift, j * perlinFactor + yShift);
                _perlinMap[i, j] = perlin;
            }
        }
        GenerateLayer(.4f);
        GenerateLayer(.6f);
        GenerateLayer(.7f);
    }

    private void GenerateLayer(float threshold)
    {
        for (int i = 0; i < _perlinMap.GetLength(0); i++)
        {
            for (int j = 0; j < _perlinMap.GetLength(1); j++)
            {
                float perlin = _perlinMap[i, j];
                if (perlin > threshold)
                {
                    var (sidesCount, dir) = GetNormalSidesCount(new Vector3Int(i, j), threshold);
                    if (sidesCount == 2)
                    {
                        Debug.Log(dir);
                        Matrix4x4 matrix; //= Matrix4x4.TRS(Vector3.zero, quaternion, new Vector3(Random.value < .5f ? 1 : -1, 1, 1));
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
                        var tileCD = new TileChangeData(new Vector3Int(i - _mapSize / 2, j - _mapSize / 2),
                            _twoSideTiles[Random.Range(0, _twoSideTiles.Length)],
                            Color.white, matrix);
                        _tilemap.SetTile(tileCD, true);
                        
                        //_tilemap.SetTile(new Vector3Int(i - _mapSize / 2, j - _mapSize / 2), _twoSideTiles[Random.Range(0, _twoSideTiles.Length)]);
                    }
                    if (sidesCount == 3)
                    {
                        Quaternion quaternion;
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
                        var tileCD = new TileChangeData(new Vector3Int(i - _mapSize / 2, j - _mapSize / 2),
                            _threeSideTiles[Random.Range(0, _threeSideTiles.Length)],
                            Color.white, matrix);
                        _tilemap.SetTile(tileCD, true);
                        //_tilemap.SetTile(new Vector3Int(i - _mapSize / 2, j - _mapSize / 2), _threeSideTiles[Random.Range(0, _threeSideTiles.Length)]);
                    }
                    if (sidesCount == 4)
                        _tilemap.SetTile(new Vector3Int(i - _mapSize / 2, j - _mapSize / 2), _fullTiles[Random.Range(0, _fullTiles.Length)]);
                }
            }
        }
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
