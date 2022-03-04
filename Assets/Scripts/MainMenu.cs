using System;
using UnityEngine;
using Zenject;

public class MainMenu : MonoBehaviour
{

    public GameObject MobGenerator;

    public GameObject PlayerObj;
    private GameData _gameData;

    [Inject]
    private void Construct(GameData gameData)
    {
        _gameData = gameData;
    }

    private void Start()
    {
        Time.timeScale = 1f;
        if (_gameData.State == GameState.Restart)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        PlayerObj.SetActive(value: true);
        new Timer(.1f).SetEnd(() => _gameData.State = GameState.InGame);
        MobGenerator.SetActive(true);
        gameObject.SetActive(false);
        Score.CurrentScore = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }
}
