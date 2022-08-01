using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class EndScreen : BaseLayer
{
    private GameData _gameData;
    private AsyncReactiveProperty<Player> _player;
    private DamageController _damageController;
    [SerializeField] private TMP_Text text;
    private Timer _timer;

    [Inject]
    private void Construct(GameData gameData, AsyncReactiveProperty<Player> player, DamageController damageController)
    {
        _gameData = gameData;
        _player = player;
        _gameData.GameStateChanged += OnGameStateChanged;
        _gameData.GameStarted += GameStarted;
        _damageController = damageController;
        damageController.Die += CheckGameOver;
    }
    protected override void OnDestroy()
    {
        _damageController.Die -= CheckGameOver;
        _gameData.GameStateChanged -= OnGameStateChanged;
        base.OnDestroy();
    }

    private void GameStarted()
    {
        _timer?.Stop();
        _timer = new Timer(960).SetEnd(() =>
        {
            gameObject.SetActive(true);
            text.text = "COMPLETE";
            Time.timeScale = 0;
        });
    }

    private void OnGameStateChanged(GameState gameState)
    {
        if (gameState == GameState.GameOver)
        {
            UniTaskAsyncEnumerable.Timer(TimeSpan.FromSeconds(1), ignoreTimeScale: true).Subscribe(_ =>
            {
                gameObject.SetActive(true);
                text.text = "GAME OVER";
                _timer?.Stop();
            });
        }
    }

    private void CheckGameOver(DamageMessage msg)
    {
        if ((IDamagable)_player.Value == msg.Target)
        {
            _gameData.State = GameState.GameOver;
        }
    }

    public void Restart()
    {
        _gameData.Reset();
        _gameData.State = GameState.Restart;
        Hide();
        UniTaskAsyncEnumerable.Timer(TimeSpan.FromSeconds(.1f), ignoreTimeScale: true).Subscribe(_ => _gameData.StartGame().Forget());
    }

    /* private void Update()
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
     }*/
}
