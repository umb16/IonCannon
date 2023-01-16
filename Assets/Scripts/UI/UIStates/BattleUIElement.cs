using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[Serializable]
public class ButtonState
{
    public Button Button;
    public UIStates State;
}

public class BattleUIElement : MonoBehaviour
{
    public UIStates EnableStates;
    //public UIStates IgnoreStates;
    public ButtonState[] ButtonStates;

    [Inject]
    private void Construct(GameData gameData)
    {
        UniTaskAsyncEnumerable.EveryValueChanged(gameData, x => x.UIStatus).Subscribe(OnStateChanged);
        foreach (var item in ButtonStates)
        {
            item.Button.onClick.AddListener(() => { gameData.UIStatus = item.State; });
        }
    }

    private void OnStateChanged(UIStates state)
    {
        gameObject.SetActive(EnableStates.HasFlag(state));
        //if (!IgnoreStates.HasFlag(state))
        //    gameObject.SetActive(EnableStates.HasFlag(state) && freeModeResult);
    }
}
