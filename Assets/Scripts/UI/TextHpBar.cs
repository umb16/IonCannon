using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

public class TextHpBar : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player)
    {
        player.Where(x => x != null).ForEachAsync(x =>
        {
            ComplexStat HP = x.StatsCollection.GetStat(StatType.HP);
            ComplexStat MaxHP = x.StatsCollection.GetStat(StatType.MaxHP);
            //_progressBar.Set(HP.Value / MaxHP.Value);
            UpdateText(HP.IntValue, MaxHP.IntValue);
            HP.ValueChanged += (x) =>
            {
                UpdateText(x.IntValue, MaxHP.IntValue);
            };
            MaxHP.ValueChanged += (x) =>
            {
                UpdateText(x.IntValue, MaxHP.IntValue);
            };
            //MaxHP.ValueChanged += (x) => _progressBar.Set(HP.Value / MaxHP.Value);
        });
    }
    private void UpdateText(int value, int max)
    {
        string fullSquares = "";
        if (value / 2 > 0)
            fullSquares += string.Concat(Enumerable.Repeat("<color=#F5025F>|</color>", value / 2));
        _text.text = "HP " +
                fullSquares +
                (value % 2 == 1 ? "<color=#fc5d9a>|</color>" : "") +
                string.Concat(Enumerable.Repeat("<color=black>|</color>", (max - value) / 2));
    }
}
