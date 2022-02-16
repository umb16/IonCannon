public class StatModificator
{
    private static int _idGenerator;

    public int Id { get; private set; }
    public StatModificatorType Type { get; private set; }
    public float Value { get; private set; }
    public StatType StatType { get; private set; }

    public StatModificator(float value, StatModificatorType modificatorType, StatType statType)
    {
        Id = ++_idGenerator;
        Value = value;
        Type = modificatorType;
        StatType = statType;
    }

    public StatModificator(StatModificator statModificator)
    {
        Id = ++_idGenerator;
        Value = statModificator.Value;
        Type = statModificator.Type;
        StatType = statModificator.StatType;
    }
}
