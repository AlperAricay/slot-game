using Gameplay.Signals;
using Gameplay.SlotModule.Model;
using UnityEngine;
using Zenject;

namespace Gameplay.SlotModule.Controller
{
    public class ParticleController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _coinParticle;

        [Inject] private SignalBus _signalBus;
        [Inject] private ProbabilitySet _probabilitySet;

        private void Awake() => Subscribe();

        private void OnDestroy() => Unsubscribe();

        private void OnSpinCompleted(SpinCompletedSignal signal)
        {
            if (!signal.Result.DoesContainSameTypes()) return;
            PlayCoinParticle();

            return;
            void PlayCoinParticle()
            {
                _coinParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                var emissionModule = _coinParticle.emission;
                emissionModule.rateOverTime =
                    _probabilitySet.GetSlotObjectByType(signal.Result.SlotObjects[0]).PrizeValue * 10;
                _coinParticle.Play();
            }
        }

        #region Listeners

        private void Subscribe()
        {
            _signalBus.Subscribe<SpinCompletedSignal>(OnSpinCompleted);
        }

        private void Unsubscribe()
        {
            _signalBus.TryUnsubscribe<SpinCompletedSignal>(OnSpinCompleted);
        }

        #endregion
    }
}
