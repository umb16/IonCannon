using System;

[Flags]
public enum DamageSources
{
    Unknown = 0,
    Ray = 1,
    Melee = 2,
    Explosion = 4,
    Ionization = 8,
    Heal = 16,
    RayInitial = 32,
    RayAll = RayInitial + Ray,
    Self = 64,
    God = 128,
    Physical = 11,
    Electricity = 12
}
