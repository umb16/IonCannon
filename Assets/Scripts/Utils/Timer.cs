using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class Timer
{
    private static bool _initialized;
    private static List<Timer> _timers = new List<Timer>();

    private bool _paused;
    private float _delay;
    private float _interval;
    private float _currentTime;
    private Action<float> _everyUpdate;
    private Action _end;

    public bool IsEnd { private set; get; }
    public float NormalizedPlayTime => Mathf.Clamp01((_currentTime - _delay) / _interval);

    public Timer(Action<float> everyUpdate, Action end, float interval, float delay = 0)
    {
        _everyUpdate = everyUpdate;
        _end = end;
        _interval = interval;
        _delay = delay;
        _timers.Add(this);
        TryStartLoop();
    }
    public Timer(Action end, float interval, float delay = 0)
    {
        _everyUpdate = null;
        _end = end;
        _interval = interval;
        _delay = delay;
        _timers.Add(this);
        TryStartLoop();
    }

    private static void TryStartLoop()
    {
        if (!_initialized)
        {
            _initialized = true;
            UpdateLoop().Forget();
        }
    }

    public void Update()
    {
        if (!_paused)
        {
            _currentTime += Time.deltaTime;

            if (_currentTime > _delay)
            {
                _everyUpdate?.Invoke(NormalizedPlayTime);
            }

            if (_currentTime - _delay > _interval)
            {
                _end?.Invoke();
                IsEnd = true;
                _timers.Remove(this);
            }
        }
    }

    public void Restart()
    {
        IsEnd = false;
        _timers.Add(this);
        _currentTime = 0;
    }

    public void Pause()
    {
        _paused = true;
    }

    public void Stop()
    {
        _timers.Remove(this);
        IsEnd = true;
    }
    public void ForceEnd()
    {
        _currentTime = _interval + 1;
        Update();
    }
    private static async UniTask UpdateLoop()
    {
        await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate())
        {
            for (int i = 0; i < _timers.Count; i++)
            {
                Timer timer = _timers[i];
                timer.Update();
            }
        }
    }
}
