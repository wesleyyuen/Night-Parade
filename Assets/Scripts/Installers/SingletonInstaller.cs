using UnityEngine;
using Zenject;

public class SingletonInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // Container.BindInterfacesAndSelfTo<InputManager>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<EventManager>().AsSingle().NonLazy();
        // Container.Bind<IEventManager>().To<EventManager>().AsSingle();
    }
}