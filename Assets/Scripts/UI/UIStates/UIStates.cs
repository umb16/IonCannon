using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum UIStates
{
    StartMenu = 1,
    Shop = 2,
    EscMenu = 4,
    Lobby = 8,
    Console = 16,
    Gameplay = 32,
    GameOver = 64,
}
