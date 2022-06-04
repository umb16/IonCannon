using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private MobSpawner _mobSpawner;
    [SerializeField] private ShopShip _shopShip;
    
    public override void InstallBindings()
    {
        Container.Bind<MobSpawner>().FromInstance(_mobSpawner).AsSingle();
        Container.Bind<ShopShip>().FromInstance(_shopShip).AsSingle();

    }
}