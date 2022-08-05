using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProgressBarStat : MonoBehaviour
{
    [SerializeField] private StatType _maxValueStatType;
    [SerializeField] private StatType _valueStatType;
    private ComplexStat _maxStat;
    private ComplexStat _valueStat;


    private void SetScale()
    {
        Debug.Log("_valueStat.Value " + _valueStat.Value);
        Debug.Log("_maxStat.Value " + _maxStat.Value);
        Debug.Log("_valueStat.Value / _maxStat.Value " + _valueStat.Value / _maxStat.Value);
        transform.localScale = new Vector3(Mathf.Max(0, _valueStat.Value / _maxStat.Value), 1, 1);
    }

    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player)
    {
        Debug.Log("sss");
        player.Where(x => x != null).ForEachAsync(x =>
        {
            _maxStat = x.StatsCollection.GetStat(_maxValueStatType);
            _valueStat = x.StatsCollection.GetStat(_valueStatType);
            SetScale();
            _maxStat.ValueChanged += _ => SetScale();
            _valueStat.ValueChanged += _ => SetScale();
        });
    }
}
