using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks.Linq;

public class ComboMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject _root;
    [SerializeField] private ProgressBar _progressBar;
    private Player _player;

    [Inject]
    private void Construct(Player player)
    {
        _player = player;
    }

    private void Awake()
    {
        _root.SetActive(false);
    }

    private void Update()
    {
        //_progressBar.Set(_player.Exp.NormalizedComboTime);
        //_root.SetActive(_player.Exp.NormalizedComboTime != 0);
        //_text.text = "X" + _player.Exp.ComboFactor;
    }

}
