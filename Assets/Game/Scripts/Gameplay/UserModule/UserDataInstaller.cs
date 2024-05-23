using Zenject;

namespace Gameplay.UserModule
{
    public class UserDataInstaller : Installer<UserDataInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UserData>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UserDataController>().AsSingle().NonLazy();
        }
    }
}