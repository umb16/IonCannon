using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class WarningSound : MonoBehaviour
{
    [SerializeField] AudioMixer _mixer;
    [SerializeField] private AudioSource _audioSource;
    private ComplexStat _lifeSupport;
    private float _oldSupportValue;
    private float _nextPlayTime;
    private float _lowpassMax = 22000;
    private float _lowpassSoftMin = 300;
    private float _lowpassMin = 100;
    private float _currentLowpass = 0;
    private bool _outOfRange = false;
    private Timer _timer;
    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player)
    {
        player.Where(x => x != null).ForEachAsync(x =>
        {
            _lifeSupport = x.StatsCollection.GetStat(StatType.LifeSupport);
            _oldSupportValue = _lifeSupport.Value;
        });

    }

    private void OnInZone()
    {
        _timer?.Stop();
        _timer = new Timer(.5f)
            .SetUpdate(x =>
            {
                _currentLowpass = Mathf.Lerp(_lowpassMax-10000, _lowpassSoftMin, (1.0f - Mathf.Pow((1.0f - x), 2 * 1.4f)));
                _mixer.SetFloat("Lowpass", _currentLowpass);
            });
    }

    private void OnOutZone()
    {
        _timer?.Stop();
        _timer = new Timer(1f)
            .SetUpdate(x => _mixer.SetFloat("Lowpass", Mathf.Lerp(_currentLowpass, _lowpassMax, x * x)));
    }

    private void Update()
    {
        if (_lifeSupport == null)
            return;
        if (_lifeSupport.Value < _oldSupportValue)
        {
            if (!_outOfRange)
            {
                _outOfRange = true;
                OnInZone();
            }
            if (_nextPlayTime < Time.time)
            {
                _audioSource.Play();
                _nextPlayTime = Time.time + _lifeSupport.Value;
            }
        }
        else
        {
            if (_outOfRange)
            {
                _outOfRange = false;
                OnOutZone();
            }
        }
        _oldSupportValue = _lifeSupport.Value;
    }
}
