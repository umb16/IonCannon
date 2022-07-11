using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilesModifier : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    // Start is called before the first frame update
    [ContextMenu("Remove")]
    void Remove()
    {
        _tilemap.SetTile(Vector3Int.zero, null);
    }
}
