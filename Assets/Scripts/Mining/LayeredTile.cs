using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
public class LayeredTile : IDamageable
{
    public bool IsEmpty => Layers.All(x => x.HP <= 0);
    public TileType TileType => GetCurrentTile()?.Type ?? TileType.Layer4;
    private List<GameObject> _linkedObjects = new List<GameObject>();
    public Tile[] Layers;
    private Tilemap[] _tilemaps;
    private Vector2Int _coords;

    public LayeredTile(Tile[] layers, Tilemap[] tilemaps, Vector2Int coords, IEnumerable<GameObject> gameObjects = null)
    {
        Layers = layers;
        _tilemaps = tilemaps;
        _coords = coords;
        if (gameObjects != null)
            _linkedObjects.AddRange(gameObjects);
    }

    public void LinkObject(GameObject gameObject)
    {
        _linkedObjects.Add(gameObject);
    }

    public Tile GetCurrentTile()
    {
        for (int i = 0; i < Layers.Length; i++)
        {
            if (Layers[i].HP > 0)
            {
                return Layers[i];
            }
        }
        return null;
    }
    public void ReceiveDamage(DamageMessage message)
    {
        for (int i = 0; i < Layers.Length; i++)
        {
            if (Layers[i].HP > 0)
            {
                Layers[i].HP -= message.Damage;
                if (Layers[i].HP <= 0)
                {
                    if (i == 0)
                        _linkedObjects.ForEach(x => GameObject.Destroy(x));
                    _tilemaps[i].SetTile((Vector3Int)_coords, null);
                    if (Layers[i].Drop.GameObject != null)
                        Layers[i].Drop.Release((Vector3)(Vector3Int)_coords * 2 + new Vector3(1 - Random.Range(-.5f, .5f), 1 - Random.Range(-.5f, .5f), 0));
                }
                return;
            }
        }
    }
}
