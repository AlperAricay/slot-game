using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Generic;
using Gameplay.SlotModule.View;
using Gameplay.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.SlotModule.Model
{
    public class SlotColumn
    {
        private readonly SlotObjectView[] _slotObjectViews;
        private readonly ProbabilitySet _probabilitySet;
        private bool _isStopping;
        private int _columnIndex;

        public SlotColumn(SlotObjectView[] slotObjectViews, int columnIndex, ProbabilitySet probabilitySet)
        {
            _slotObjectViews = slotObjectViews;
            _columnIndex = columnIndex;
            _probabilitySet = probabilitySet;
            
            Initialize();
        }

        private void Initialize()
        {
            foreach (var slotObjectView in _slotObjectViews) 
                slotObjectView.SetData(_probabilitySet.SlotObjects.GetRandomElement());
        }

        public async UniTask Spin(float spinDuration, Combination spinResult, StopType stopType)
        {
            ToggleBlurs(true);
            
            var timeToStartStopping = spinDuration - GameConfig.GetStopDuration(stopType);
            var elapsedTime = 0f;
            while (elapsedTime < timeToStartStopping)
            {
                foreach (var slotObjectView in _slotObjectViews)
                {
                    if (!slotObjectView.Advance(GameConfig.SpinAdvanceDuration)) 
                        slotObjectView.SetData(_probabilitySet.SlotObjects.GetRandomElement());
                }
                await UniTask.WaitUntil(HasAllSlotsStopped);
                elapsedTime += GameConfig.SpinAdvanceDuration;
            }

            var resultingObject = _probabilitySet.GetSlotObjectByType(spinResult.SlotObjects[_columnIndex]);
            await Stop(stopType, resultingObject);
        }

        private async UniTask Stop(StopType stopType, SlotObject resultingObject)
        {
            var topMostView = _slotObjectViews.First(view => view.PosIndex == 0);
            topMostView.SetData(resultingObject);
            
            if (stopType != StopType.Fast) ToggleBlurs(false);
            var stopDuration = GameConfig.GetStopDuration(stopType);
            await AdvanceSlotObjects(stopDuration / 2);
            await AdvanceSlotObjects(stopDuration / 2);
            if (stopType == StopType.Fast) ToggleBlurs(false);
        }

        private async UniTask AdvanceSlotObjects(float duration, Ease ease = Ease.Linear)
        {
            foreach (var slotObjectView in _slotObjectViews)
            {
                if (!slotObjectView.Advance(duration, ease))
                    slotObjectView.SetData(_probabilitySet.SlotObjects.GetRandomElement());
            }
            await UniTask.WaitUntil(HasAllSlotsStopped);
        }

        private void ToggleBlurs(bool value)
        {
            foreach (var slotObjectView in _slotObjectViews) 
                slotObjectView.ToggleBlur(value);
        }
        
        private bool HasAllSlotsStopped() => !_slotObjectViews.Any(view => view.IsSpinning);
        
        public enum StopType
        {
            Fast,
            Normal,
            Slow
        }
    }
}
