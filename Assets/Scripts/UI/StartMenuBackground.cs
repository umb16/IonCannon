using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

public class StartMenuBackground : MonoBehaviour
{
    private float AspectRatio => Screen.width / (float)Screen.height;
    [Inject]
    private void Construct(GameData gameData)
    {
        gameData.GameStarted += OnGameStarted;
        gameData.GameStateChanged += GameStateChanged;
        UniTaskAsyncEnumerable.EveryValueChanged(this, x => x.AspectRatio).Subscribe(UpdateSize);
    }

    private void GameStateChanged(GameState state)
    {
        if (state == GameState.StartMenu)
            gameObject.SetActive(true);
    }

    private void OnGameStarted()
    {
        gameObject.SetActive(false);
    }

    private void UpdateSize(float ratio)
    {
        //float ratio = Screen.width / (float)Screen.height;

        if (ratio > 1.77777f)
        {
            float zeroToOne = (ratio - 1.77777f) / (2.439294f - 1.77777f);
            transform.localScale = Vector3.one * Mathf.LerpUnclamped(1.095f, 1.51f, zeroToOne);
            transform.localPosition = new Vector3(0, Mathf.LerpUnclamped(0, -6.8f, zeroToOne), transform.localPosition.z);

        }
        else
        {
            transform.localScale = Vector3.one * 1.095f;
            transform.localPosition = new Vector3(0, 0, transform.localPosition.z);
        }
    }

}
