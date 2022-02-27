using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private Player _player;
    public override void InstallBindings()
    {
        Container.Bind<Player>().FromInstance(_player).AsSingle().NonLazy();
        Container.Bind<PlayerPerksController>().AsSingle().NonLazy();
    }
}