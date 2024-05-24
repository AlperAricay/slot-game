using Gameplay.SlotModule.Model;

namespace Gameplay.Signals
{
    public struct SpinCompletedSignal
    {
        public readonly Combination Result;

        public SpinCompletedSignal(Combination result)
        {
            Result = result;
        }
    }
}
