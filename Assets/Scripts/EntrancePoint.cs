using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Zenject;

public class EntrancePoint : MonoBehaviour
{
    private GameData _gameData;

    [Inject]
    private void Construct(GameData gameData)
    {
        _gameData = gameData;
    }
    // Start is called before the first frame update
    void Start()
    {
        _gameData.SetState(GameState.StartMenu);
        Application.targetFrameRate = 60;
    }

}
