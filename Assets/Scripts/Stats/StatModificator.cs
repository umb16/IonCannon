using System;
using System.Globalization;

public class StatModificator : IStatModificator
{
    public StatModificatorType Type { get; private set; }
    public float Value { get; private set; }
    public StatType StatType { get; private set; }
    public Func<float, float> Func { get; private set; }

    public override string ToString()
    {
        string statName = "";
        switch (StatType)
        {
            case StatType.MovementSpeed:
                statName = LocaleKeys.Main.MOVE_SPEED.GetLocalizedString();
                break;
            case StatType.RaySpeed:
                statName = LocaleKeys.Main.RAY_SPEED.GetLocalizedString();
                break;
            case StatType.Energy:
                statName = LocaleKeys.Main.RAY_PATH_LENGHT.GetLocalizedString();
                break;
            case StatType.RayDelay:
                statName = LocaleKeys.Main.RAY_CHARGE_DELAY.GetLocalizedString();
                break;
            case StatType.RayDamageAreaRadius:
                statName = LocaleKeys.Main.RAY_WIDTH.GetLocalizedString();
                break;
            case StatType.RayDamage:
                statName = LocaleKeys.Main.RAY_DAMAGE.GetLocalizedString();
                break;
            case StatType.HP:
                statName = LocaleKeys.Main.HP.GetLocalizedString();
                break;
            case StatType.MaxHP:
                statName = LocaleKeys.Main.MAX_HP.GetLocalizedString();
                break;
            case StatType.Score:
                break;
            case StatType.Size:
                break;
            case StatType.LifeSupport:
                break;
            case StatType.Damage:
                break;
            case StatType.PickupRadius:
                statName = LocaleKeys.Main.PICKUP_RADIUS.GetLocalizedString();
                break;
            case StatType.RayError:
                statName = LocaleKeys.Main.RAY_ERROR.GetLocalizedString();
                break;
            case StatType.Defence:
                break;
            case StatType.RayReverse:
                break;
            case StatType.Capacity:
                statName = LocaleKeys.Main.Capacity.GetLocalizedString();
                break;
            case StatType.EnergyRegen:
                statName = LocaleKeys.Main.EnergyRegen.GetLocalizedString();
                break;
            default:
                break;
        }
        if (string.IsNullOrEmpty(statName))
            return "";
        string value = "";
        switch (Type)
        {
            case StatModificatorType.Additive:
                value = Value.ToString("#.#", new CultureInfo("en-US", false));
                break;
            case StatModificatorType.Multiplicative:
                value = (Value * 100).ToString("#.#", new CultureInfo("en-US", false)) + "%";
                break;
            case StatModificatorType.TransformChain:
                return "";
            case StatModificatorType.Correction:
                return "";
            default:
                return "";
        }
        string perfix = Value > 0 ? "+" : "";
        return statName + " " + perfix + value;
    }

    public StatModificator(float value, StatModificatorType modificatorType, StatType statType)
    {
        Value = value;
        Type = modificatorType;
        StatType = statType;
    }
    public StatModificator(Func<float, float> func, StatModificatorType modificatorType, StatType statType)
    {
        Type = modificatorType;
        StatType = statType;
        Func = func;
    }
}
