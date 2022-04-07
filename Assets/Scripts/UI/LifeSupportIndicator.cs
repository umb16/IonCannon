using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LifeSupportIndicator : MonoBehaviour
{
    [SerializeField] private ProgressBar _progressBar;
    private ComplexStat _lifeSupportStat;

    [Inject]
    private void Construct(Player player)
    {
        _lifeSupportStat = player.StatsCollection.GetStat(StatType.LifeSupport);
        _progressBar.Set(_lifeSupportStat.Value);
        _lifeSupportStat.ValueChanged += (x) => _progressBar.Set(x.Value);
    }
}
