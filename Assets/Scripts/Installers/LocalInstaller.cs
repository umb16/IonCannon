using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "LocalInstaller", menuName = "Installers/LocalInstaller")]
public class LocalInstaller : ScriptableObjectInstaller<LocalInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<LocalizationManager>().FromInstance(new LocalizationManager()).AsSingle().NonLazy();
    }
}