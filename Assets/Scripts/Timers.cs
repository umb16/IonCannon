using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class Timers : MonoBehaviour
{
    private static bool _initialized;
    private static Timers _instance;
    private static Timers Instance
    {
        get
        {
            if (!_initialized)
            {
                _instance = new GameObject("Timer").AddComponent<Timers>();
                _initialized = true;
            }
            return _instance;
        }
    }

    private readonly List<Timer> _timers = new List<Timer>();


    private void OnDestroy()
    {
        _initialized = false;
    }


    public class Timer
    {
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
            Instance._timers.Add(this);
        }
        public Timer(Action end, float interval, float delay = 0)
        {
            _everyUpdate = null;
            _end = end;
            _interval = interval;
            _delay = delay;
            Instance._timers.Add(this);
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
                    Instance._timers.Remove(this);
                }
            }
        }

        public void Restart()
        {
            IsEnd = false;
            Instance._timers.Add(this);
            _currentTime = 0;
        }

        public void Pause()
        {
            _paused = true;
        }

        public void Stop()
        {
            Instance._timers.Remove(this);
            IsEnd = true;
        }
        public void ForceEnd()
        {
            _currentTime = _interval + 1;
            Update();
        }
    }


    private void Update()
    {
        for (int i = 0; i < _timers.Count; i++)
        {
            Timer timer = _timers[i];
            timer.Update();
        }
    }
}
