using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LifeSupportIndicator : MonoBehaviour
{
    [SerializeField] private ProgressBar _progressBar;
    [SerializeField] private StatType _statType;
    private ComplexStat _lifeSupportStat;

    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player)
    {
        player.Where(x => x != null).ForEachAsync(x =>
        {
            _lifeSupportStat = x.StatsCollection.GetStat(_statType);
            _progressBar.Set(_lifeSupportStat.Value);
            _lifeSupportStat.ValueChanged += (x) => _progressBar.Set(x.Value);
        });
    }
}
