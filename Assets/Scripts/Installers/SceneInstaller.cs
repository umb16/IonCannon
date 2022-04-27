using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private Player _player;
    [SerializeField] private MobSpawner _mobSpawner;
    [SerializeField] private CooldownsPanel _cooldownsPanel;
    [SerializeField] private UIShop _uiShop;
    [SerializeField] private UIPlayerInventory _uiPlayerInventory;
    [SerializeField] private UIPlayerStats _uiPlayerStats;
    public override void InstallBindings()
    {
        Container.Bind<Player>().FromInstance(_player).AsSingle().NonLazy();
        
        Container.Bind<DamageController>().AsSingle().NonLazy();
        Container.Bind<GameData>().AsSingle().NonLazy();
        Container.Bind<MobSpawner>().FromInstance(_mobSpawner).AsSingle();
        Container.Bind<CooldownsPanel>().FromInstance(_cooldownsPanel).AsSingle();
        Container.Bind<UIShop>().FromInstance(_uiShop).AsSingle();
        Container.Bind<UIPlayerInventory>().FromInstance(_uiPlayerInventory).AsSingle();
        Container.Bind<UIPlayerStats>().FromInstance(_uiPlayerStats).AsSingle();
    }
}