using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    private List<Layer> _layers = new List<Layer>();
    private float[,] _perlinMap;
    private int _xShift;
    private int _yShift;
    private float _perlinFactor;
    public int Size { get; private set; }

    public Chunk(int size, int xShift, int yShift, float perlinFactor)
    {
        Size = size;
        _xShift = xShift;
        _yShift = yShift;
        _perlinFactor = perlinFactor;
        _perlinMap = new float[size, size];
        for (int i = 0; i < _perlinMap.GetLength(0); i++)
        {
            for (int j = 0; j < _perlinMap.GetLength(1); j++)
            {
                float perlin = GetPerlin(i, j);
                _perlinMap[i, j] = perlin;
            }
        }
    }
    public float GetPerlin(int i, int j)
    {
        return Mathf.PerlinNoise((i + _xShift) * _perlinFactor, (j + _yShift) * _perlinFactor);
    }
    public float GetСachedPerlin(int i, int j)
    {
        if (i < 0 || i >= Size || j < 0 || j >= Size)
            return GetPerlin(i, j);
        return _perlinMap[i, j];
    }
    public void CreateLayer(float threshold, TileType tileType)
    {
            _layers.Add(new Layer(this, threshold, TileType.Standart));
        if (_layers.Count > 1)
            _layers.Sort((a, b) => b.Threshold.CompareTo(a.Threshold));
    }

    public (TileType type, Directions direction) GetTile(Vector2Int position)
    {
        foreach (var layer in _layers)
        {
            if (layer.ValidTiles.TryGetValue(position, out var tilesDirections))
                return (layer.TileType, tilesDirections);
        }
        return (TileType.Standart, Directions.ALL);
    }
    public Layer GetLayer()
    {
        return _layers[0];
    }
}
