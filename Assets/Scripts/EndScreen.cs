using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private GameObject text;
    private GameData _gameData;
    private Player _player;
    private DamageController _damageController;

    [Inject]
    private void Construct(GameData gameData, Player player, DamageController damageController)
    {
        _gameData = gameData;
        _player = player;
        _gameData.GameStateChanged += OnGameStateChanged;
        _damageController = damageController;
        damageController.Die += CheckGameOver;
    }
    private void OnDestroy()
    {
        _damageController.Die -= CheckGameOver;
        _gameData.GameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState gameState)
    {
        if (gameState == GameState.GameOver)
            text.SetActive(true);
    }

    private void CheckGameOver(DamageMessage msg)
    {
        if (_player.ID == msg.Target.ID)
        {
            _gameData.State = GameState.GameOver;
        }
    }

    private void Update()
    {
        if (_gameData.State == GameState.GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _gameData.Reset();
                _gameData.State = GameState.Restart;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
