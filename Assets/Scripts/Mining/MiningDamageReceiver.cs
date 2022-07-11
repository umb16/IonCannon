using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayeredTile : IDamagable
{
    private int[] layersHp; 
    public void ReceiveDamage(DamageMessage message)
    {
        throw new System.NotImplementedException();
    }
}

public class MiningDamageReceiver : MonoBehaviour
{
    private Dictionary<Vector2Int,LayeredTile> _tiles = new Dictionary<Vector2Int, LayeredTile>();
    public IDamagable Check(Vector3 position, float radius)
    {
        Vector2Int x = new Vector2Int((int)position.x, (int)position.y);
        if (_tiles.TryGetValue(x, out var layer))
            return layer;
        else
        {
            var result = new LayeredTile();
            _tiles[x] = result;
            return result;
        }
    }
}
