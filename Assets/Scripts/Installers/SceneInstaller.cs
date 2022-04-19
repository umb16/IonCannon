using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private Player _player;
    [SerializeField] private MobSpawner _mobSpawner;
    [SerializeField] private CooldownsPanel _cooldownsPanel;
    public override void InstallBindings()
    {
        Container.Bind<Player>().FromInstance(_player).AsSingle().NonLazy();
        
        Container.Bind<DamageController>().AsSingle().NonLazy();
        Container.Bind<GameData>().AsSingle().NonLazy();
        Container.Bind<MobSpawner>().FromInstance(_mobSpawner).AsSingle();
        Container.Bind<CooldownsPanel>().FromInstance(_cooldownsPanel).AsSingle();

    }
}