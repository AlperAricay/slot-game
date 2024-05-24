using Gameplay.SerializationModule;
using Gameplay.Signals;
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
            InstallSignals();

            Container.InstantiateComponentOnNewGameObject<ApplicationStateAnnouncer>();
        }

        private void InstallSignals()
        {
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<SpinCompletedSignal>().OptionalSubscriber();
        }
    }
}
