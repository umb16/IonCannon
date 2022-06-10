using SPVD.LifeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Assets.Scripts.Objects.Mobs.Enemies
{
    internal class MobArtillery : EnemySimple
    {
        private LifeSupportTower _lifeSupportTower;

        [Inject]
        private void Construct(LifeSupportTower lifeSupportTower)
        {
            _lifeSupportTower = lifeSupportTower;

        }


    }
}
