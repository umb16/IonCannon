using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HPIndicator : MonoBehaviour
{
    [SerializeField] private ProgressBar _progressBar;

    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player)
    {
        player.Where(x => x != null).ForEachAsync(x =>
        {
            ComplexStat HP = x.StatsCollection.GetStat(StatType.HP);
            ComplexStat MaxHP = x.StatsCollection.GetStat(StatType.MaxHP);
            _progressBar.Set(HP.Value / MaxHP.Value);
            HP.ValueChanged += (x) => _progressBar.Set(HP.Value / MaxHP.Value);
            MaxHP.ValueChanged += (x) => _progressBar.Set(HP.Value / MaxHP.Value);
        });
    }
}
