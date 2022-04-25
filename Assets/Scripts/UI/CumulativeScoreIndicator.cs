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
    private Player _player;

    [Inject]
    private void Construct(Player player)
    {
        _player = player;
        _player.Gold.ValueChanged += x => _text.text = "Ресурсы: " + x.Value;
    }
}
