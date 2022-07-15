using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class MiningDamageReceiver : MonoBehaviour
{
    [SerializeField] private Tilemap[] _tilemaps;
    [SerializeField] private TileBase _tile;
    [SerializeField] private TileBase _rareTile;
    [SerializeField] private Drop[] _drops;
    public Tiles Tiles { get; private set; }

    [Inject]
    private void Construct(GameData gameData)
    {
        gameData.GameStateChanged += GameStateChanged;
    }

    private void GameStateChanged(GameState state)
    {
        if (state == GameState.Restart)
        {
            Start();
        }
    }

    private void Start()
    {
        Tiles = new Tiles(_tilemaps, _tile, _drops, _rareTile);
    }
}
