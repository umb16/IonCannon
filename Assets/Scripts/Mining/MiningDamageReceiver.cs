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
    [SerializeField] private PlantsCollection _plants;
    public Tiles Tiles { get; private set; }

    [Inject]
    private void Construct(GameData gameData)
    {
        gameData.GameStarted += Generate;
       // gameData.GameStateChanged += GameStateChanged;
    }

    private void GameStateChanged(GameState state)
    {
        if (state == GameState.Restart)
        {
            Generate();
        }
    }

    private void Generate()
    {
        Tiles = new Tiles(_tilemaps, _tile, _drops, _rareTile, _plants);
    }
}
