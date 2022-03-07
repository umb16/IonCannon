using UnityEngine;
using Zenject;

public class OriginInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        //Container.Bind<DamageController>().AsSingle().NonLazy();
        //Container.Bind<GameData>().AsSingle().NonLazy();
    }
}