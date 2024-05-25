using Gameplay.SlotModule.Controller;
using Gameplay.SlotModule.Model;
using Gameplay.UI;
using UnityEngine;
using Zenject;

namespace Gameplay.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private ProbabilityConfig _probabilityConfig;
        [SerializeField] private ProbabilitySet _probabilitySet;
        [SerializeField] private SlotSpinButton _slotSpinButton;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_probabilityConfig).AsSingle().NonLazy();
            Container.BindInstance(_probabilitySet).AsSingle().NonLazy();
            Container.BindInstance(_slotSpinButton).AsSingle().NonLazy();
            Container.Bind<ProbabilityController>().AsSingle().NonLazy();
        }
    }
}