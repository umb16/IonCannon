using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

abstract public class PerkBase : WithTag, IPerk
{
    virtual public bool IsCommon { get; } = false;

    public PerkType Type { get; private set; } = PerkType.Base;

    abstract public void Add(IPerk perk);

    public new PerkBase AddTag(Tags tag)
    {
        base.AddTag(tag);
        return this;
    }

    public PerkBase SetType(PerkType type)
    {
        Type = type;
        return this;
    }

    virtual public string GetDescription()
    {
        return "EmptyDescription";
    }

    abstract public void Init(IMob mob);

    abstract public void Shutdown();
}
