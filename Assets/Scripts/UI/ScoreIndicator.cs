using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class ScoreIndicator : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player)
    {
        player.Where(x => x != null).ForEachAsync(x =>
        {
            _text.text = "0";
            x.Gold.ValueChanged += x =>
            {
                _text.text = x.Value.ToString();
            };
        });
    }
}