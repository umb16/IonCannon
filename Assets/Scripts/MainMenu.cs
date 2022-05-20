using System;
using UnityEngine;
using Zenject;

public class MainMenu : BaseLayer
{
    private GameData _gameData;
    private MobSpawner _mobSpawner;
    private Player _player;

    [Inject]
    private void Construct(GameData gameData, MobSpawner mobSpawner, Player player)
    {
        _gameData = gameData;
        _mobSpawner = mobSpawner;
        _player = player;
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
        _player.gameObject.SetActive(value: true);
        new Timer(.1f).SetEnd(() => _gameData.StartGame());
        _mobSpawner.gameObject.SetActive(true);
        //gameObject.SetActive(false);
        Show<InGameHUDLayer>();
        TryGet<MainMenu>().Hide();
        //Hide();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }
}
