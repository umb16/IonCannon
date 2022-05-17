using System.Collections.Generic;
using System.Linq;
using Zenject;

public class PerksFactory
{
    private DiContainer _container;
    private SceneContextRegistry _sceneContextRegistry;

    public PerksFactory(DiContainer container, SceneContextRegistry sceneContextRegistry)
    {
        _container = container;
        _sceneContextRegistry = sceneContextRegistry;
    }

    public IPerk Create<T>() where T : IPerk
    {
        return _sceneContextRegistry.SceneContexts.First().Container.Instantiate<T>();
    }
    public IPerk Create<T>(IEnumerable<object> extraArgs) where T : IPerk
    {
        return _sceneContextRegistry.SceneContexts.First().Container.Instantiate<T>(extraArgs);
    }
}
