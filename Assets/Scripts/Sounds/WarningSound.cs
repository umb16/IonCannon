using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WarningSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    private ComplexStat _lifeSupport;
    private float _oldSupportValue;
    private float _nextPlayTime;
    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player)
    {
        player.Where(x => x != null).ForEachAsync(x =>
        {
            _lifeSupport = x.StatsCollection.GetStat(StatType.LifeSupport);
            _oldSupportValue = _lifeSupport.Value;
        });

    }

    private void Update()
    {
        if (_lifeSupport == null)
            return;
        if (_lifeSupport.Value < _oldSupportValue)
        {
            /*if (_oldSupportValue == 1)
                _audioSource.Play();*/
            if (_nextPlayTime < Time.time)
            {
                _audioSource.Play();
                _nextPlayTime = Time.time + _lifeSupport.Value;
            }
        }
        _oldSupportValue = _lifeSupport.Value;
    }
}
