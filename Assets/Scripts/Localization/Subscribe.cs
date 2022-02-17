using System;

public class Subscribe
{
    public Action Action;
    public Func<bool> UsubscribeCondition;

    public Subscribe(Action action, Func<bool> usubscribeCondition)
    {
        Action = action;
        UsubscribeCondition = usubscribeCondition;
    }
}
