using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

public class MainMenu : BaseLayer
{
    private GameData _gameData;

    [Inject]
    private void Construct(GameData gameData)
    {
        _gameData = gameData;
        _gameData.GameStateChanged += GameStateChanged;
    }

    private void GameStateChanged(GameState obj)
    {
        if (obj == GameState.StartMenu)
        {
            gameObject.SetActive(true);
        }
    }

    private void OnEnabled()
    {
        Time.timeScale = 1f;
        /*if (_gameData.State == GameState.Restart)
        {
            StartGame();
        }*/
    }

    public void StartGame()
    {
        /*new Timer(.1f).SetEnd(() =>*/
        //_gameData.StartGame().Forget();//);
        //Show<InGameHUDLayer>();
        Show<LobbyUI>();
        Hide();
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.Space) && !CheatPanelLayer.Enabled)
        {
            StartGame();
        }*/
    }
}
