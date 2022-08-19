using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class HelpMessageLayer : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private float _nextUpdate;
    private MobSpawner _mobSpawner;
    private Vector2 _statCursorPos;
    private Vector2 _endCursorPos;
    [Inject]
    private void Construct(GameData gameData, MobSpawner mobSpawner)
    {
        gameObject.SetActive(false);
        gameData.GameStateChanged += GameStateChanged;
        gameData.GameStarted += GameStarted;
        _mobSpawner = mobSpawner;
    }

    private void GameStateChanged(GameState obj)
    {
        if (obj == GameState.Gameplay)
        {
            if(_mobSpawner.Stop)
                gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void GameStarted()
    {
        _mobSpawner.Stop = true;
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _statCursorPos = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(0))
        {
            _endCursorPos = Input.mousePosition;
            if ((_endCursorPos - _statCursorPos).magnitude > 20)
            {
                gameObject.SetActive(false);
                _mobSpawner.Stop = false;
            }
        }
        if (_nextUpdate < Time.time)
        {
            _nextUpdate = Time.time + 1f;
            //text.enabled = !text.enabled;
        }
    }
}
