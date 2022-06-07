using Cysharp.Threading.Tasks;
using SPVD.LifeSupport;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private MobSpawner _mobSpawner;
    [SerializeField] private ShopShip _shopShip;
    [SerializeField] private LifeSupportTower _lifeSupportTower;
    
    public override void InstallBindings()
    {
        Container.Bind<MobSpawner>().FromInstance(_mobSpawner).AsSingle();
        Container.Bind<ShopShip>().FromInstance(_shopShip).AsSingle();
        Container.Bind<LifeSupportTower>().FromInstance(_lifeSupportTower).AsSingle();

    }
}