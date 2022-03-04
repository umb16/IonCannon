using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private GameObject text;
    private GameData _gameData;

    [Inject]
    private void Construct(GameData gameData, Player player, DamageController damageController)
    {
        _gameData = gameData;
        _gameData.GameStateChanged += x =>
        {
            if (x == GameState.GameOver)
                text.SetActive(true);
        };

        damageController.Die += (msg) =>
        {
            if (player.ID == msg.Target.ID)
            {
                _gameData.State = GameState.GameOver;
            }
        };
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
