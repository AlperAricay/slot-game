using Zenject;

namespace Gameplay.SerializationModule
{
    public class SerializationInstaller : Installer<SerializationInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<NewtonsoftJsonSerializationService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<FileSerializationController>().AsSingle().NonLazy();
        }
    }
}