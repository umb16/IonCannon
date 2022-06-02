using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class EndScreen : BaseLayer
{
    [SerializeField] private GameObject text;
    private GameData _gameData;
    private AsyncReactiveProperty<Player> _player;
    private DamageController _damageController;

    [Inject]
    private void Construct(GameData gameData, AsyncReactiveProperty<Player> player, DamageController damageController)
    {
        _gameData = gameData;
        _player = player;
        _gameData.GameStateChanged += OnGameStateChanged;
        _damageController = damageController;
        damageController.Die += CheckGameOver;
    }
    protected override void OnDestroy()
    {
        _damageController.Die -= CheckGameOver;
        _gameData.GameStateChanged -= OnGameStateChanged;
        base.OnDestroy();
    }

    private void OnGameStateChanged(GameState gameState)
    {
        if (gameState == GameState.GameOver)
        {
            gameObject.SetActive(true);
            text.SetActive(true);
        }
    }

    private void CheckGameOver(DamageMessage msg)
    {
        if (_player.Value.ID == msg.Target.ID)
        {
            _gameData.State = GameState.GameOver;
        }
    }

    private void Update()
    {
        if (_gameData.State == GameState.GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !CheatPanelLayer.Enabled )
            {
                _gameData.Reset();
                _gameData.State = GameState.Restart;
                Hide();
                UniTaskAsyncEnumerable.Timer(TimeSpan.FromSeconds(.1f), ignoreTimeScale: true).Subscribe(_ => _gameData.StartGame().Forget());
                //new Timer(.1f).SetEnd(() => _gameData.StartGame().Forget());
                //Time.timeScale = 1;
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
