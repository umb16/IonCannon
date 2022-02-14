using System;
using System.Collections.Generic;
using UnityEngine;

public class Timers : MonoBehaviour
{
	public class Timer
	{
		public Action<Timer> Func;

		public float CurrentDelay;

		public float Delay;

		public bool IsFrameTimer;

		public float TimeFromStart;

		public float StartTime;

		public float PlayTime;

		public float NormalizedPlayTime => Mathf.Min(1f, (TimeFromStart - StartTime) / PlayTime);

		public Timer(Action<Timer> func, float delay, bool isFrameTimer = false)
		{
			Func = func;
			CurrentDelay = delay;
			Delay = delay;
			IsFrameTimer = isFrameTimer;
			Self._timers.Add(this);
		}

		public Timer(Action<Timer> func, float startTime, float playTime, float delay = 0f, Action endAnimAction = null, bool isFrameTimer = false)
		{
			StartTime = startTime;
			PlayTime = playTime;
			Func = delegate(Timer timer)
			{
				func(timer);
				if (NormalizedPlayTime == 1f)
				{
					timer.Stop();
					if (endAnimAction != null)
					{
						endAnimAction();
					}
				}
			};
			CurrentDelay = startTime;
			Delay = delay;
			IsFrameTimer = isFrameTimer;
			Self._timers.Add(this);
		}

		public Timer(Action func, float delay, bool isFrameTimer = false)
		{
			Func = delegate(Timer timer)
			{
				func();
				timer.Stop();
			};
			CurrentDelay = delay;
			Delay = delay;
			IsFrameTimer = isFrameTimer;
			Self._timers.Add(this);
		}

		public void Stop()
		{
			Self._timers.Remove(this);
		}
	}

	private static Timers _self;

	private readonly List<Timer> _timers = new List<Timer>();

	public static Timers Self
	{
		get
		{
			if (_self == null)
			{
				new GameObject("Timer").AddComponent<Timers>();
			}
			return _self;
		}
	}

	private void Awake()
	{
		_self = this;
	}

	private void Update()
	{
		for (int i = 0; i < _timers.Count; i++)
		{
			Timer timer = _timers[i];
			if (timer.IsFrameTimer)
			{
				timer.CurrentDelay -= 1f;
				timer.TimeFromStart += 1f;
			}
			else
			{
				timer.CurrentDelay -= Time.deltaTime;
				timer.TimeFromStart += Time.deltaTime;
			}
			if (timer.CurrentDelay < 0f)
			{
				timer.Func(timer);
				timer.CurrentDelay += timer.Delay;
			}
		}
	}
}
