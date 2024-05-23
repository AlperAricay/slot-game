using Gameplay.SerializationModule;
using Zenject;

namespace Gameplay.Installers
{
    public class AppInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SerializationInstaller.Install(Container);
        }
    }
}
