using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public static class Waves
    {

    public static LevelEvent[] StartWaves = {

        new SpawnEvent(1, 20, Addresses.Mob_First, 2).SetDirection(0,360),
        new SpawnEvent(21, 231, Addresses.Mob_First, 1).SetDirection(0,360),
        new SpawnEvent(121, 182, Addresses.Mob_SpdBuff, 20).SetDirection(0,360),
        new SpawnEvent(196, 227, Addresses.Mob_SpdBuff, 15).SetDirection(0,360),

    };

    public static LevelEvent[] RingWaves = {

        new SpawnEvent(61, Addresses.Mob_First).SetFixedCount(18).SetDirection(0,360),
        new SpawnEvent(121, Addresses.Mob_First).SetFixedCount(22).SetDirection(0,360),
        new SpawnEvent(181, Addresses.Mob_First).SetFixedCount(26).SetDirection(0,360),
        new SpawnEvent(301, Addresses.Mob_First).SetFixedCount(26).SetDirection(0,360),
        new SpawnEvent(361, Addresses.Mob_First).SetFixedCount(32).SetDirection(0,360),
        new SpawnEvent(421, Addresses.Mob_First).SetFixedCount(38).SetDirection(0,360),
        new SpawnEvent(541, Addresses.Mob_First).SetFixedCount(38).SetDirection(0,360),
        new SpawnEvent(601, Addresses.Mob_First).SetFixedCount(44).SetDirection(0,360),
        new SpawnEvent(661, Addresses.Mob_First).SetFixedCount(50).SetDirection(0,360),
    };


    public static LevelEvent[] FirstBlock_1 = {

        new SpawnEvent(241, 270, Addresses.Mob_First, 2).SetDirection(0,360),
        new SpawnEvent(271, 471, Addresses.Mob_First, 1).SetDirection(0,360),
        new SpawnEvent(241, 330, Addresses.Mob_Child, 10).SetDirection(0,360),
        new SpawnEvent(331, 465, Addresses.Mob_Child, 7).SetDirection(0,360),
        new SpawnEvent(271, 330, Addresses.Mob_Artillery, 15).SetDirection(0,360),
        new SpawnEvent(331, 462, Addresses.Mob_Artillery, 10).SetDirection(0,360),
    };

    public static LevelEvent[] FirstBlock_2 = {

        new SpawnEvent(241, 270, Addresses.Mob_First, 2).SetDirection(0,360),
        new SpawnEvent(271, 471, Addresses.Mob_First, 1).SetDirection(0,360),
        new SpawnEvent(241, 330, Addresses.Mob_Child, 10).SetDirection(0,360),
        new SpawnEvent(331, 464, Addresses.Mob_Child, 7).SetDirection(0,360),
        new SpawnEvent(251, 420, Addresses.Mob_Slowdowner, 8).SetDirection(0,360),
    };

    public static LevelEvent[] SecondBlock_1 = {

        new SpawnEvent(481, 506, Addresses.Mob_Child, 4).SetDirection(0,360),
        new SpawnEvent(509, 567, Addresses.Mob_Child, 3).SetDirection(0,360),
        new SpawnEvent(569, 660, Addresses.Mob_Child, 2).SetDirection(0,360),
        new SpawnEvent(661, 711, Addresses.Mob_Child, 1).SetDirection(0,360),
        new SpawnEvent(491, 692, Addresses.Mob_Fortress, 20).SetDirection(0,360),


    };


    public static LevelEvent[] ThirdBlock_1 = {


        new SpawnEvent(721, 780, Addresses.Mob_Child, 2).SetDirection(0,360),
        new SpawnEvent(781, 951, Addresses.Mob_Child, 1).SetDirection(0,360),
        new SpawnEvent(731, 780, Addresses.Mob_Artillery, 10).SetDirection(0,360),
        new SpawnEvent(781, 838, Addresses.Mob_Artillery, 7).SetDirection(0,360),
        new SpawnEvent(842, 898, Addresses.Mob_Artillery, 5).SetDirection(0,360),
        new SpawnEvent(901, 949, Addresses.Mob_Artillery, 6).SetDirection(0,360),
        new SpawnEvent(721, 780, Addresses.Mob_Slowdowner, 5).SetDirection(0,360),
        new SpawnEvent(781, 840, Addresses.Mob_Slowdowner, 3).SetDirection(0,360),
        new SpawnEvent(841, 900, Addresses.Mob_Slowdowner, 2).SetDirection(0,360),

    };

    public static LevelEvent[] ThirdBlock_2 = {


        //хеви спидбаф чилдрен

    };
}
