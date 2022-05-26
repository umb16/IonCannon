using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class CumulativeScoreIndicator : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player)
    {
        player.Where(x => x != null).ForEachAsync(x=>
        {
            _text.text = "Ресурсы: 0";
            x.Gold.ValueChanged += x =>
            {
                _text.text = "Ресурсы: " + x.Value;
            };
        });
        //player.Gold.ValueChanged += x => _text.text = "Ресурсы: " + x.Value;
    }
}
