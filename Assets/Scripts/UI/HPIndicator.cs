using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HPIndicator : MonoBehaviour
{
    [SerializeField] private ProgressBar _progressBar;

    [Inject]
    private void Construct(Player player)
    {
        ComplexStat HP = player.StatsCollection.GetStat(StatType.HP);
        ComplexStat MaxHP = player.StatsCollection.GetStat(StatType.MaxHP);
        _progressBar.Set(HP.Value / MaxHP.Value);
        HP.ValueChanged += (x) => _progressBar.Set(HP.Value / MaxHP.Value);
        MaxHP.ValueChanged += (x) => _progressBar.Set(HP.Value / MaxHP.Value);
    }
}
