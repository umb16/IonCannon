using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private Player _player;
    [SerializeField] private PerksMenu _perksMenu;
    public override void InstallBindings()
    {
        Container.Bind<PerksMenu>().FromInstance(_perksMenu).AsSingle().NonLazy();
        Container.Bind<Player>().FromInstance(_player).AsSingle().NonLazy();
        Container.Bind<PlayerPerksController>().AsSingle();
    }
}