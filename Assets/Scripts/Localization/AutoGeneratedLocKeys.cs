//autogenerated
public enum LocKeys
{
    //Играть
    Play,
    //Нет
    None,
    //Скорость бега
    MoveSpeed,
    //Скорость луча
    RaySpeed,
    //Длинна пути луча
    RayPathLenght,
    //Скорость зарядки луча
    RayChargeDelay,
    //Ширина луча
    RayDamageAreaRadius,
    //Урон луча
    RayDamage,
    //Ур.
    Lvl,
    //Нажмите пробел чтобы начать
    PressSpaceToStart,
    //Комбо
    Combo,
    //Взрывающийся ящик
    ExplosiveBox,
    //Ионизация
    Ionization,
}
public static class LocKeyConverter
{
    public static string Convert(LocKeys key)
    {
        switch (key)
        {
            case LocKeys.Play:
                return "Play";
            case LocKeys.None:
                return "None";
            case LocKeys.MoveSpeed:
                return "MoveSpeed";
            case LocKeys.RaySpeed:
                return "RaySpeed";
            case LocKeys.RayPathLenght:
                return "RayPathLenght";
            case LocKeys.RayChargeDelay:
                return "RayChargeDelay";
            case LocKeys.RayDamageAreaRadius:
                return "RayDamageAreaRadius";
            case LocKeys.RayDamage:
                return "RayDamage";
            case LocKeys.Lvl:
                return "Lvl";
            case LocKeys.PressSpaceToStart:
                return "PressSpaceToStart";
            case LocKeys.Combo:
                return "Combo";
            case LocKeys.ExplosiveBox:
                return "ExplosiveBox";
            case LocKeys.Ionization:
                return "Ionization";
                default: return "";
        }
    }
}