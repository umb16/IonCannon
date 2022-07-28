using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class LevelEventsFactory
{
    List<Func<LevelEvent[]>> _funcs = new List<Func<LevelEvent[]>>();
    public LevelEventsFactory AddEvents(params LevelEvent[][] levelEvents)
    {
        _funcs.Add(() => levelEvents[Random.Range(0, levelEvents.Length)]);
        return this;
    }
    public LevelEvent[] Get()
    {
        var list = new List<LevelEvent>();
        foreach (var func in _funcs)
        {
            list.AddRange(func());
        }
        foreach (var item in list)
        {
            item.Reset();
        }
        return list.ToArray();
    }
}
