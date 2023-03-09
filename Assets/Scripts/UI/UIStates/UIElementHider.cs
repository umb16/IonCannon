using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[Serializable]
public class UIElementHider : MonoBehaviour
{
    public UIStates EnableStates;

    [Inject]
    private void Construct(GameData gameData)
    {
        UniTaskAsyncEnumerable.EveryValueChanged(gameData, x => x.UIStatus).Subscribe(OnStateChanged);
    }

    private void OnStateChanged(UIStates state)
    {
        gameObject.SetActive(EnableStates.HasFlag(state));
    }
}
