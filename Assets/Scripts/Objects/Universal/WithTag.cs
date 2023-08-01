using System.Collections.Generic;

public class WithTag : WithId
{
    private readonly HashSet<Tags> _tags = new();
    public void AddTag(Tags tag)
    {
        _tags.Add(tag);
    }
    public bool ContainsTag(Tags tag)
    {
       return _tags.Contains(tag);
    }
}
