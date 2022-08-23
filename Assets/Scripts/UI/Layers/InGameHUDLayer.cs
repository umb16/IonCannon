using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InGameHUDLayer : UIElement
{
    [Inject]
    private void Construct(GameData gameData)
    {
        gameData.GameStateChanged += GameStateChanged;
    }

    private void GameStateChanged(GameState obj)
    {
        if (obj == GameState.StartMenu)
            Hide();
    }
}
