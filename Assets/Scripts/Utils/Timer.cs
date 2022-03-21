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
    private float _time;
    private float _currentTime;
    private Action<float> _everyUpdate;
    private Action _end;
    private Func<bool> _condition;
    public float TimeToEnd => _delay + _time - _currentTime;
    public bool IsEnd { private set; get; }
    public float NormalizedPlayTime => Mathf.Clamp01((_currentTime - _delay) / _time);
    Comparison<Timer> _timeToEndComparsion = (x, y) => x.TimeToEnd.CompareTo(y.TimeToEnd);

    public Timer(float interval)
    {
        _time = interval;
        _timers.Add(this);
       
        _timers.Sort(_timeToEndComparsion);
        TryStartLoop();
    }


    public Timer SetUpdate(Action<float> everyUpdate)
    {
        _everyUpdate = everyUpdate;
        _everyUpdate?.Invoke(0);
        return this;
    }
    public Timer SetEnd(Action end)
    {
        _end = end;
        return this;
    }
    public Timer SetDelay(float delay)
    {
        _delay = delay;
        _timers.Sort(_timeToEndComparsion);
        return this;
    }
    public Timer SetCondition(Func<bool> condition)
    {
        _condition = condition;
        return this;
    }


    private static void TryStartLoop()
    {
        if (!_initialized)
        {
            _initialized = true;
            UpdateLoop().Forget();
        }
    }

    public bool Update()
    {
        if (!_paused)
        {
            _currentTime += Time.deltaTime;

            if (_currentTime > _delay)
            {
                _everyUpdate?.Invoke(NormalizedPlayTime);
            }

            if (_currentTime - _delay > _time)
            {
                _end?.Invoke();
                IsEnd = true;
                _timers.Remove(this);
                return false;
            }
        }
        return true;
    }

    public void Pause()
    {
        _paused = true;
    }

    public void Stop()
    {
        if (!IsEnd)
        {
            _timers.Remove(this);
            IsEnd = true;
        }
    }
    public void ForceEnd()
    {
        if (!IsEnd)
        {
            _currentTime = _time + 1;
            Update();
        }
    }
    private static async UniTask UpdateLoop()
    {
        await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate())
        {
            for (int i = 0; i < _timers.Count; i++)
            {
                Timer timer = _timers[i];
                if (!timer.Update())
                {
                    i--;
                }
            }
        }
    }
}
