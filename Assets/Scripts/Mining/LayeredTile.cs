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
                    if (_drops.Length > i)
                        _drops[i].Release((Vector3)(Vector3Int)_coords * 2 + new Vector3(1 - Random.Range(-.5f, .5f), 1 - Random.Range(-.5f, .5f), 0));
                }
                return;
            }
        }
    }
}
