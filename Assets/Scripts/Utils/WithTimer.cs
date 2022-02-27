using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithTimer : MonoBehaviour
{
    private List<Timer> _timers = new List<Timer>();
    internal Timer CreateTimer(float interval)
    {
        var timer = new Timer(interval);
        _timers.Add(timer);
        return timer;
    }

    private void OnDestroy()
    {
        foreach (var timer in _timers)
        {
            timer.Stop();
        }
    }
}
