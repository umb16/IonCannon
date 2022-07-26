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
    }

    private void Start()
    {
        Time.timeScale = 1f;
        if (_gameData.State == GameState.Restart)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        /*new Timer(.1f).SetEnd(() =>*/
        _gameData.StartGame().Forget();//);
        Show<InGameHUDLayer>();
        TryGet<MainMenu>().Hide();
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
