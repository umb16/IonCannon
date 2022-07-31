using Cysharp.Threading.Tasks;
using SPVD.LifeSupport;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private MobSpawner _mobSpawner;
    [SerializeField] private ShopShip _shopShip;
    [SerializeField] private LifeSupportTower _lifeSupportTower;
    [SerializeField] private MiningDamageReceiver _miningDamageReceiver;
    [SerializeField] private FakeCursor _fakeCursor;
    
    public override void InstallBindings()
    {
        Container.Bind<MobSpawner>().FromInstance(_mobSpawner).AsSingle();
        Container.Bind<ShopShip>().FromInstance(_shopShip).AsSingle();
        Container.Bind<LifeSupportTower>().FromInstance(_lifeSupportTower).AsSingle();
        Container.Bind<MiningDamageReceiver>().FromInstance(_miningDamageReceiver).AsSingle();
        Container.Bind<FakeCursor>().FromInstance(_fakeCursor).AsSingle();

    }
}