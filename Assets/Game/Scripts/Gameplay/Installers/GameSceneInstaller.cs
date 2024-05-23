using Gameplay.SlotModule.Controller;
using Gameplay.SlotModule.Model;
using UnityEngine;
using Zenject;

namespace Gameplay.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private ProbabilityConfig _probabilityConfig;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_probabilityConfig).AsSingle().NonLazy();
            Container.Bind<ProbabilityController>().AsSingle().NonLazy();
        }
    }
}