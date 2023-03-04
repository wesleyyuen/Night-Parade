using UnityEngine;
using Zenject;

public class SingletonInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<InputManager>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<EventManager>().AsSingle().NonLazy();
        // Container.Bind<EventManager>().To<EventManager>().AsSingle();
    }
}