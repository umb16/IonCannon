using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tiles
{
    private Array2DWithNegatives<LayeredTile> _tiles = new Array2DWithNegatives<LayeredTile>(50, 50, 50, 50);
    private float _cellRadius = 1.4f;

    public Tiles(Tilemap[] tilemaps, TileBase tile, Drop[] drops, TileBase rare)
    {
        float shiftx = Random.value * 1000 + 1000;
        float shifty = Random.value * 1000 + 1000;
        for (int i = _tiles.NX; i < _tiles.PX; i++)
        {
            for (int j = _tiles.NY; j < _tiles.PY; j++)
            {
                
                
                var hp = new[] { 20f, 100f, 300f };
                //float randomValue = Random.value;
                float randomValue = Mathf.PerlinNoise((i*.1f+ shiftx /*+ _xShift*/) /* _perlinFactor*/, (j *.1f+ shifty/*+ _yShift*/) /* _perlinFactor*/);
                for (int x = 0; x < tilemaps.Length; x++)
                {
                    Tilemap map = tilemaps[x];
                    if (x==2 && Random.value < .003f)
                    {
                        map.SetTile(new Vector3Int(i, j), rare);
                        hp[x] = 350;
                    }
                    else
                    {
                        if (randomValue * (x + 1) > .3f)
                            map.SetTile(new Vector3Int(i, j), tile);
                        else
                        {
                            hp[x] = 0;
                            map.SetTile(new Vector3Int(i, j), null);
                        }
                        //randomValue += Random.value;
                    }
                }
                _tiles[i, j] = new LayeredTile(hp, tilemaps, new Vector2Int(i, j), drops);
            }
        }
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
