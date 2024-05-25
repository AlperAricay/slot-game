using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.Generic;
using Gameplay.Signals;
using Gameplay.SlotModule.Model;
using Gameplay.SlotModule.View;
using Gameplay.UI;
using Gameplay.UserModule;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

// ReSharper disable PossibleLossOfFraction

namespace Gameplay.SlotModule.Controller
{
    public class SlotMachineController : MonoBehaviour
    {
        [Inject] private UserData _userData;
        [Inject] private ProbabilityController _probabilityController;
        [Inject] private ProbabilityConfig _probabilityConfig;
        [Inject] private ProbabilitySet _probabilitySet;
        [Inject] private SlotSpinButton _spinButton;
        [Inject] private SignalBus _signalBus;

        [SerializeField] private SlotObjectView _slotObjectViewPrefab;

        private SlotColumn[] _slotColumns;
        private bool _isSpinning;

        public void Initialize()
        {
            ValidateSpinData();
            InitializeSlotObjects();
            Subscribe();

            return;

            void InitializeSlotObjects()
            {
                var columnCount = _userData.SpinData[_userData.LastSpinIndex].SlotObjects.Length;
                _slotColumns = new SlotColumn[columnCount];

                const int slotObjectPerColumn = 7;
                var yPositions = new float[slotObjectPerColumn];
                for (int i = 0; i < slotObjectPerColumn; i++)
                    yPositions[i] = GameConfig.VerticalSlotOffset * (slotObjectPerColumn / 2 - i);

                var currentXPos = -GameConfig.HorizontalSlotOffset * (columnCount / 2);
                for (int i = 0; i < columnCount; i++)
                {
                    var slotObjectViews = new SlotObjectView[slotObjectPerColumn];
                    var currentYPos = -GameConfig.VerticalSlotOffset * (slotObjectPerColumn / 2);
                    for (int j = 0; j < slotObjectPerColumn; j++)
                    {
                        var slotObjectView = Instantiate(_slotObjectViewPrefab, transform);
                        slotObjectView.Initialize(yPositions, slotObjectPerColumn - 1 - j);
                        slotObjectView.transform.position = new Vector3(currentXPos, currentYPos);
                        slotObjectViews[j] = slotObjectView;
                        currentYPos += GameConfig.VerticalSlotOffset;
                    }

                    _slotColumns[i] = new SlotColumn(slotObjectViews, i, _probabilitySet);
                    currentXPos += GameConfig.HorizontalSlotOffset;
                }
            }
        }

        public void Dispose() => Unsubscribe();

        private void Subscribe() => _spinButton.onClick.AddListener(OnSpinButtonClicked);
        private void Unsubscribe() => _spinButton.onClick.RemoveListener(OnSpinButtonClicked);

        private void OnSpinButtonClicked()
        {
            if (_isSpinning) return;
            Spin(GameConfig.DesiredSpinDuration).Forget();
        }

        private async UniTaskVoid Spin(float spinDuration)
        {
            _isSpinning = true;
            ValidateSpinData();

            spinDuration -= (_slotColumns.Length - 1) * GameConfig.DelayBetweenColumnSpins;

            var resultingCombination = _userData.SpinData[_userData.LastSpinIndex];
            var lastSpinType = GetLastSpinType();

            var lastSpinDuration = GameConfig.GetStopDuration(lastSpinType);
            var remainingSpinDuration = spinDuration - lastSpinDuration;

            var tasks = new List<UniTask>();
            for (var i = 0; i < _slotColumns.Length; i++)
            {
                float targetSpinDuration;
                SlotColumn.StopType stopType;
                if (i == _slotColumns.Length - 1)
                {
                    targetSpinDuration = spinDuration;
                    stopType = lastSpinType;
                }
                else
                {
                    targetSpinDuration = remainingSpinDuration;
                    stopType = SlotColumn.StopType.Fast;
                }

                var task = _slotColumns[i].Spin(targetSpinDuration, resultingCombination, stopType);
                tasks.Add(task);
                await UniTask.Delay(TimeSpan.FromSeconds(GameConfig.DelayBetweenColumnSpins));
            }

            await UniTask.WhenAll(tasks);
            _userData.LastSpinIndex++;
            _signalBus.Fire(new SpinCompletedSignal(resultingCombination));
            _isSpinning = false;

            return;

            SlotColumn.StopType GetLastSpinType()
            {
                var stopType = SlotColumn.StopType.Fast;
                if (resultingCombination.DoesContainSameTypes())
                {
                    var randomBool = Random.Range(0, 2) == 0;
                    stopType = randomBool ? SlotColumn.StopType.Normal : SlotColumn.StopType.Slow;
                }

                return stopType;
            }
        }

        private void ValidateSpinData()
        {
            if (_userData.LastSpinIndex <= 99 && _userData.SpinData[_userData.LastSpinIndex].SlotObjects.Length ==
                _probabilityConfig.Probabilities[0].combination.SlotObjects.Length) return;

            _probabilityController.GenerateSpinData();
            _userData.LastSpinIndex = 0;
        }
    }
}