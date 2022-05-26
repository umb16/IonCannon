using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private MobSpawner _mobSpawner;
    
    public override void InstallBindings()
    {
        Container.Bind<MobSpawner>().FromInstance(_mobSpawner).AsSingle();
    }
}