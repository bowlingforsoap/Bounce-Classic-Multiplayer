using Zenject;

public class SceneMonoInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GroundManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
    }
}
