using Zenject;

namespace SaveLoad
{
    public class ProjectInstaller : MonoInstaller<ProjectInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<GameRepositoryBinary>().AsSingle();
        }
    }
}
