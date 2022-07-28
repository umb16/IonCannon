using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tiles
{
    private Array2DWithNegatives<LayeredTile> _tiles = new Array2DWithNegatives<LayeredTile>(50, 50, 50, 50);
    private float _cellRadius = 1.4f;

    public Tiles(Tilemap[] tilemaps, TileBase tile, Drop[] drops, TileBase rare, PlantsCollection plants)
    {
        float shiftx = Random.value * 1000 + 1000;
        float shifty = Random.value * 1000 + 1000;
        for (int i = _tiles.NX; i < _tiles.PX; i++)
        {
            for (int j = _tiles.NY; j < _tiles.PY; j++)
            {

                var tiles = new[] { new Tile(), new Tile(), new Tile() };
                var hp = new[] { 20f, 100f, 300f };
                //float randomValue = Random.value;
                float randomValue = Mathf.PerlinNoise((i * .1f + shiftx), (j * .1f + shifty));
                for (int x = 0; x < tilemaps.Length; x++)
                {
                    Tilemap map = tilemaps[x];
                    {
                        if (randomValue * (x + 1) > .3f && x == 0 || x > 0)
                        {
                            map.SetTile(new Vector3Int(i, j), tile);
                            tiles[x].HP = hp[x];
                            if (drops.Length > x)
                                tiles[x].Drop = drops[x];
                            switch (x)
                            {
                                case 0:
                                    tiles[x].Type = TileType.Grass;
                                    break;
                                case 1:
                                    tiles[x].Type = TileType.Layer2;
                                    break;
                                case 2:
                                    tiles[x].Type = TileType.Layer3;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            tiles[x].HP = 0;
                            map.SetTile(new Vector3Int(i, j), null);
                        }
                        //randomValue += Random.value;
                    }
                }
                float randomValue2 = Mathf.PerlinNoise((i * .1f + shiftx * 2), (j * .1f + shifty * 2));
                if (randomValue2 < .3f && tiles[0].HP > 0)
                    _tiles[i, j] = new LayeredTile(tiles, tilemaps, new Vector2Int(i, j),
                        Enumerable.Range(0, (int)(Mathf.Lerp(1, 5, (.3f - randomValue2)*3))).Select(_ => (plants.CreatePlantsPack(new Vector3(i * 2 + 1 + (Random.value * 2 - 1) * .1f, j * 2 + 1 + (Random.value * 2 - 1.1f) * .7f)))));
                else if (randomValue2 < .5f && Random.value<.01f && tiles[0].HP > 0)
                        _tiles[i, j] = new LayeredTile(tiles, tilemaps, new Vector2Int(i, j),
                        Enumerable.Range(0, 1).Select(_ => (plants.CreatePlant(new Vector3(i * 2 + 1 + (Random.value * 2 - 1) * .7f, j * 2 + 1 + (Random.value * 2 - 1.1f) * .7f)))));
                   // if (/*randomValue2 < .5f &&*/ tiles[0].HP > 0/* && Random.value<.2f*/)
                   //  _tiles[i, j] = new LayeredTile(tiles, tilemaps, new Vector2Int(i, j),
                   //      Enumerable.Range(0,10).Select(_ => (plants.CreatePlantsPack(new Vector3(i * 2+1 + (Random.value * 2 - 1) * .1f, j * 2+1 + (Random.value * 2 - 1.1f) * .7f)))));
                else
                    _tiles[i, j] = new LayeredTile(tiles, tilemaps, new Vector2Int(i, j));
            }
        }
        Tilemap map2 = tilemaps[2];
        for (int i = 0; i < 3; i++)
        {
            var pos = MathMethods.GetRandomPointInCircle(Vector2.zero, 15, 0);
            Vector2Int coords = new Vector2Int((int)pos.x, (int)pos.y);
            map2.SetTile((Vector3Int)coords, rare);
            _tiles[coords.x, coords.y].Layers[2].HP = 350;
        }
        for (int i = 0; i < 4; i++)
        {
            var pos = MathMethods.GetRandomPointInCircle(Vector2.zero, 20, 15);
            Vector2Int coords = new Vector2Int((int)pos.x, (int)pos.y);
            map2.SetTile((Vector3Int)coords, rare);
            _tiles[coords.x, coords.y].Layers[2].HP = 350;
        }
        for (int i = 0; i < 8; i++)
        {
            var pos = MathMethods.GetRandomPointInCircle(Vector2.zero, 25, 20);
            Vector2Int coords = new Vector2Int((int)pos.x, (int)pos.y);
            map2.SetTile((Vector3Int)coords, rare);
            _tiles[coords.x, coords.y].Layers[2].HP = 350;
        }
        for (int i = 0; i < 16; i++)
        {
            var pos = MathMethods.GetRandomPointInCircle(Vector2.zero, 30, 25);
            Vector2Int coords = new Vector2Int((int)pos.x, (int)pos.y);
            map2.SetTile((Vector3Int)coords, rare);
            _tiles[coords.x, coords.y].Layers[2].HP = 350;
        }
    }

    public TileType GetTileTypeByCoords(Vector2 coords)
    {
        Vector2Int scaled = new Vector2Int(Mathf.RoundToInt(coords.x * .5f), Mathf.RoundToInt(coords.y * .5f));
        if (_tiles.InBounds(scaled.x, scaled.y))
        {
            return _tiles[scaled.x, scaled.y].TileType;
        }
        return TileType.None;
    }

    public LayeredTile[] GetInRadius(Vector2 center, float radius)
    {
        List<LayeredTile> result = new List<LayeredTile>();
        List<Vector2Int> toCheck = new List<Vector2Int>();
        Vector2Int scaled = new Vector2Int(Mathf.RoundToInt(center.x * .5f), Mathf.RoundToInt(center.y * .5f));
        float sqrRadius = Mathf.Pow(_cellRadius + radius, 2);


        if (_tiles.InBounds(scaled.x, scaled.y))
        {
            result.Add(_tiles[scaled.x, scaled.y]);
            toCheck.Add(scaled);
            while (toCheck.Count > 0)
            {
                scaled = toCheck[0];
                foreach (var neighbourCoords in new Vector2Int[] { new Vector2Int(1, 0) + scaled, new Vector2Int(-1, 0) + scaled, new Vector2Int(0, 1) + scaled, new Vector2Int(0, -1) + scaled })
                {
                    if (_tiles.InBounds(neighbourCoords.x, neighbourCoords.y))
                    {
                        if ((neighbourCoords * 2 - center).sqrMagnitude < sqrRadius)
                        {
                            var neighbour = _tiles[neighbourCoords.x, neighbourCoords.y];
                            if (!result.Contains(neighbour) && !neighbour.IsEmpty)
                            {
                                result.Add(neighbour);
                                toCheck.Add(neighbourCoords);
                            }
                        }
                    }
                }
                toCheck.RemoveAt(0);
            }
        }
        return result.ToArray();
    }
}
