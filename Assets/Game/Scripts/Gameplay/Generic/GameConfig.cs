using System;
using Gameplay.SlotModule.Model;

namespace Gameplay.Generic
{
    public static class GameConfig
    {
        public const float VerticalSlotOffset = 2.15f;
        public const float HorizontalSlotOffset = 2.6f;
        public const float SpinAdvanceDuration = .05f;
        public const float FastStopDuration = .05f;
        public const float NormalStopDuration = 1f;
        public const float SlowStopDuration = 2.25f;
        public const float DelayBetweenColumnSpins = .5f;
        public const float DesiredSpinDuration = 3f;

        public static float GetStopDuration(SlotColumn.StopType stopType)
        {
            return stopType switch
            {
                SlotColumn.StopType.Fast => FastStopDuration,
                SlotColumn.StopType.Normal => NormalStopDuration,
                SlotColumn.StopType.Slow => SlowStopDuration,
                _ => throw new ArgumentOutOfRangeException(nameof(stopType), stopType, null)
            };
        }
    }
}
