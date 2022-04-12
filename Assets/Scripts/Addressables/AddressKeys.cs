//autogenerated
public enum AddressKeys
{
   Mob_First,
   Fx_Dust,
   Fx_SpeedUp,
   Fx_Aura0,
   Mob_Second,
   Fx_Explosion,
   Snd_Explosion,
   Obj_Barrel,
   Mob_SpdBuff,
   Fx_Radiation,
   Mob_Child,
   Fx_AuraBoss,
   Ico_Laser,
}
public static class AddressKeysConverter
{
    public static string Convert(AddressKeys key)
    {
        switch (key)
        {
            case AddressKeys.Mob_First:
                return "Mob_First";
            case AddressKeys.Fx_Dust:
                return "Fx_Dust";
            case AddressKeys.Fx_SpeedUp:
                return "Fx_SpeedUp";
            case AddressKeys.Fx_Aura0:
                return "Fx_Aura0";
            case AddressKeys.Mob_Second:
                return "Mob_Second";
            case AddressKeys.Fx_Explosion:
                return "Fx_Explosion";
            case AddressKeys.Snd_Explosion:
                return "Snd_Explosion";
            case AddressKeys.Obj_Barrel:
                return "Obj_Barrel";
            case AddressKeys.Mob_SpdBuff:
                return "Mob_SpdBuff";
            case AddressKeys.Fx_Radiation:
                return "Fx_Radiation";
            case AddressKeys.Mob_Child:
                return "Mob_Child";
            case AddressKeys.Fx_AuraBoss:
                return "Fx_AuraBoss";
            case AddressKeys.Ico_Laser:
                return "Ico_Laser";
                default: return "";
        }
    }
}