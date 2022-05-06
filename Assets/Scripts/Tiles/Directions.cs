using System;

namespace IonCannon.Tiles
{
    [Flags]
    public enum Directions
    {
        None = 0,
        Up = 1,
        Right = 2,
        Down = 4,
        Left = 8,

        UR = Up + Right,
        RD = Right + Down,
        DL = Down + Left,
        LU = Left + Up,

        UD = Up + Down,
        LR = Left + Right,

        URD = Up + Down + Right,
        RDL = Right + Down + Left,
        DLU = Down + Left + Up,
        LUR = Left + Up + Right,

        ALL = Up + Down + Left + Right,
    }
}