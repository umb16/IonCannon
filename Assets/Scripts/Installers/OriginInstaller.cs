using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class OriginInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        //Container.Bind<DamageController>().AsSingle().NonLazy();
        //Container.Bind<GameData>().AsSingle().NonLazy();

        Container.Bind<ItemsDB>().AsSingle();
        Container.Bind<DamageController>().AsSingle().NonLazy();
        Container.Bind<GameData>().AsSingle().NonLazy();
        Container.Bind<PerksFactory>().AsSingle();
        Container.Bind<AsyncReactiveProperty<CooldownsPanel>>().FromInstance(new AsyncReactiveProperty<CooldownsPanel>(null)).AsSingle().NonLazy();
        Container.Bind<AsyncReactiveProperty<Player>>().FromInstance(new AsyncReactiveProperty<Player>(null)).AsSingle().NonLazy();
        Container.Bind<UICooldownsManager>().AsSingle().NonLazy();
    }
}