using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private Player _player;
    [SerializeField] private PerksMenu _perksMenu;
    [SerializeField] private MobSpawner _mobSpawner;
    public override void InstallBindings()
    {
        Container.Bind<PerksMenu>().FromInstance(_perksMenu).AsSingle().NonLazy();
        Container.Bind<Player>().FromInstance(_player).AsSingle().NonLazy();
        Container.Bind<PlayerPerksController>().AsSingle();
        
        Container.Bind<DamageController>().AsSingle().NonLazy();
        Container.Bind<GameData>().AsSingle().NonLazy();
        Container.Bind<MobSpawner>().FromInstance(_mobSpawner).AsSingle();

    }
}