using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LayeredTile : IDamagable
{
    public bool IsEmpty => _layersHp.All(x => x <= 0);
    private float[] _layersHp;
    private Tilemap[] _tilemaps;
    private Vector2Int _coords;
    private Drop[] _drops;

    public LayeredTile(float[] layersHp, Tilemap[] tilemaps, Vector2Int coords, Drop[] drops)
    {
        _layersHp = layersHp;
        _tilemaps = tilemaps;
        _coords = coords;
        _drops = drops;
    }

    public void ReceiveDamage(DamageMessage message)
    {
        for (int i = 0; i < _layersHp.Length; i++)
        {
            if (_layersHp[i] > 0)
            {
                _layersHp[i] -= message.Damage;
                Debug.Log(message.Damage);
                if (_layersHp[i] <= 0)
                {
                    _tilemaps[i].SetTile((Vector3Int)_coords, null);
                    _drops[i].Release((Vector3)(Vector3Int)_coords * 2 + new Vector3(1 - Random.Range(-.5f,.5f), 1 - Random.Range(-.5f, .5f), 0));
                }
                return;
            }
        }
    }
}

public class Array2DWithNegatives<T>
{
    public int PX { get; private set; }
    public int PY { get; private set; }
    public int NX { get; private set; }
    public int NY { get; private set; }

    private T[,] _p;
    private T[,] _n;
    private T[,] _pn;
    private T[,] _np;

    public Array2DWithNegatives(int positiveX, int positiveY, int negativeX, int negativeY)
    {
        PX = positiveX;
        PY = positiveY;
        NX = -negativeX;
        NY = -negativeY;
        _p = new T[positiveX, positiveY];
        _pn = new T[positiveX, negativeY];
        _np = new T[negativeX, positiveY];
        _n = new T[negativeX, negativeY];
    }
    public bool InBounds(int x, int y)
    {
        return x >= NX && y >= NY && x < PX && y < PY;
    }

    public T this[int index1, int index2]
    {
        get
        {
            if (index1 >= 0 && index2 >= 0)
                return _p[index1, index2];
            if (index1 < 0 && index2 >= 0)
                return _np[-(index1 + 1), index2];
            if (index1 >= 0 && index2 < 0)
                return _pn[index1, -(index2 + 1)];
            return _n[-(index1 + 1), -(index2 + 1)];
        }
        set
        {

            if (index1 >= 0 && index2 >= 0)
                _p[index1, index2] = value;
            else if (index1 < 0 && index2 >= 0)
                _np[-(index1 + 1), index2] = value;
            else if (index1 >= 0 && index2 < 0)
                _pn[index1, -(index2 + 1)] = value;
            else
                _n[-(index1 + 1), -(index2 + 1)] = value;
        }
    }
}

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
                    if (x==0 && Random.value < .003f)
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

public class MiningDamageReceiver : MonoBehaviour
{
    [SerializeField] private Tilemap[] _tilemaps;
    [SerializeField] private TileBase _tile;
    [SerializeField] private TileBase _rareTile;
    [SerializeField] private Drop[] _drops;
    public Tiles Tiles { get; private set; }
    private void Start()
    {
        Tiles = new Tiles(_tilemaps, _tile, _drops, _rareTile);
        /*for (int i = 0; i < 100; i++)
        {
            foreach (var item in tiles.GetInRadius(new Vector2(Random.Range(-50f,50f), Random.Range(-50f, 50f)), 5.0f))
            {

                item.ReceiveDamage(new DamageMessage(null, item, 10000, DamageSources.Ray));
            }

        }*/
    }
}
