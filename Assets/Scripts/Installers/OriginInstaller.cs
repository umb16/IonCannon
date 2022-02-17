using UnityEngine;
using Zenject;

public class OriginInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        var collection = StatsCollectionsDB.StandartPlayer();
        Container.Bind<IStatsCollection>().FromInstance(collection).AsSingle().NonLazy();
    }
}