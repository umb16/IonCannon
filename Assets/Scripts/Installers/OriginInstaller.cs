using UnityEngine;
using Zenject;

public class OriginInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<LocalizationManager>().FromInstance(new LocalizationManager()).AsSingle().NonLazy();

        var collection = StatsCollectionsDB.StandartPlayer();
        Container.Bind<IStatsCollection>().FromInstance(collection).AsSingle().NonLazy();
    }
}