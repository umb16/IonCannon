using System;
using System.Collections.Generic;

public class Subscriber
{
    List<Subscribe> _subscribers = new List<Subscribe>();
    public void Add(Action action, Func<bool> condition)
    {
        _subscribers.Add(new Subscribe(action, condition));
    }

    public void Invoke()
    {
        for (int i = 0; i < _subscribers.Count; i++)
        {
            Subscribe subscriber = _subscribers[i];
            if (subscriber.UsubscribeCondition())
            {
                _subscribers.RemoveAt(i);
                i--;
            }
            else
            {
                subscriber.Action.Invoke();
            }
        }
    }
}
