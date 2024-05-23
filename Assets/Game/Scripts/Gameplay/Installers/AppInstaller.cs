using Gameplay.SerializationModule;
using Gameplay.UserModule;
using Gameplay.Utility;
using Zenject;

namespace Gameplay.Installers
{
    public class AppInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SerializationInstaller.Install(Container);
            UserDataInstaller.Install(Container);

            Container.InstantiateComponentOnNewGameObject<ApplicationStateAnnouncer>();
        }
    }
}
