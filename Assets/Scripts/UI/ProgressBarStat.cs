using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ProgressBarStat : MonoBehaviour
{
    [SerializeField] private StatType _maxValueStatType;
    [SerializeField] private StatType _valueStatType;
    [SerializeField] private Image _image;
    private ComplexStat _maxStat;
    private ComplexStat _valueStat;

    private void SetScale()
    {
        if (_valueStat.Value / _maxStat.Value < .3f)
        {
            _image.color = Color.Lerp(new Color(1, 0, 0, .5f), new Color(1, 1, 1, .5f), Mathf.Pow(_valueStat.Value / _maxStat.Value * 3,2));
        }
        else
        {
            _image.color = new Color(1, 1, 1, .5f);
        }
        if (_image != null)
            _image.fillAmount = Mathf.Max(0, _valueStat.Value / _maxStat.Value);
        else
            transform.localScale = new Vector3(Mathf.Max(0, _valueStat.Value / _maxStat.Value), 1, 1);
    }

    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player)
    {
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
