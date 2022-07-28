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

        new SpawnEvent(61, Addresses.Mob_First).SetFixedCount(20).SetDirection(0,360),
        new SpawnEvent(121, Addresses.Mob_First).SetFixedCount(26).SetDirection(0,360),
        new SpawnEvent(181, Addresses.Mob_First).SetFixedCount(32).SetDirection(0,360),

        new SpawnEvent(330, Addresses.Mob_First).SetFixedCount(40).SetDirection(0,360),
        new SpawnEvent(390, Addresses.Mob_First).SetFixedCount(40).SetDirection(0,360),
        new SpawnEvent(450, Addresses.Mob_First).SetFixedCount(40).SetDirection(0,360),

        new SpawnEvent(540, Addresses.Mob_First).SetFixedCount(40).SetDirection(0,360),
        new SpawnEvent(600, Addresses.Mob_First).SetFixedCount(40).SetDirection(0,360),
        new SpawnEvent(660, Addresses.Mob_First).SetFixedCount(40).SetDirection(0,360),
    };


    public static LevelEvent[] _levelEventsX2 = {

        new SpawnEvent(0, 20, Addresses.Mob_First, 2).SetDirection(0,360),
        new SpawnEvent(21, 240, Addresses.Mob_First, 1).SetDirection(0,360),
        new SpawnEvent(120, 200, Addresses.Mob_SpdBuff, 20).SetDirection(0,360),
        new SpawnEvent(210, 240, Addresses.Mob_SpdBuff, 15).SetDirection(0,360),
       
        //после 4 минуты
        new SpawnEvent(241, 300, Addresses.Mob_First, 1).SetDirection(0,360),
        new SpawnEvent(300, 480, Addresses.Mob_First, 0.6f).SetDirection(0,360),
        new SpawnEvent(261, 480, Addresses.Mob_Child, 7).SetDirection(0,360),
        new SpawnEvent(260, 480, Addresses.Mob_Artillery, 10).SetDirection(0,360),

        

        //после 8 минуты

        new SpawnEvent(481, 720, Addresses.Mob_Child, 2).SetDirection(0,360),
        new SpawnEvent(500, 720, Addresses.Mob_Second, 8).SetDirection(0,360),

        

        //после 9 минуты

        new SpawnEvent(721, 960, Addresses.Mob_First, 1).SetDirection(0,360),
        new SpawnEvent(735, 960, Addresses.Mob_Artillery, 10).SetDirection(0,360),
        new SpawnEvent(735, 960, Addresses.Mob_Slowdowner, 6).SetDirection(0,360),

    };
    

}
