using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum UIStates
{
    StartMenu = 1,
    Shop = 2,
    Pause = 4,
    Lobby = 8,
    Console = 16,
    Play = 32,
    GameOver = 64,
}
