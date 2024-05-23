using System;
using System.Collections.Generic;
using System.Linq;

namespace Gameplay.SlotModule.Model
{
    [Serializable]
    public class Probability
    {
        public Interval[] Intervals { get; set; }

        public Combination combination;
        public float probability;

        public void Initialize()
        {
            CalculateIntervals();

            return;
            void CalculateIntervals()
            {
                var summedFrequency = GetFrequency();
                var roundedSummedFrequency = Round(summedFrequency);

                Intervals = new Interval[(int)probability];
                Intervals[0] = new Interval(new Limits(0, roundedSummedFrequency - 1));

                for (int i = 1; i < Intervals.Length; i++)
                {
                    summedFrequency += GetFrequency();
                    roundedSummedFrequency = Round(Math.Min(summedFrequency, 100));

                    var upperLimit = roundedSummedFrequency - 1;
                    var lowerLimit = Intervals[i - 1].Limits.UpperLimit + 1;

                    var limits = new Limits(lowerLimit, upperLimit);
                    Intervals[i] = new Interval(limits);

                    if (roundedSummedFrequency == 100) break;
                }
            }
        }
        
        public float GetFrequency() => 100 / probability;
        public Combination GetCombination() => combination;

        public int GetAlternativeCountToIndex(int index, List<int> usedIndexes)
        {
            var targetInterval = GetTargetInterval(index);
            if (targetInterval == null) return -1;

            return targetInterval.Limits.GetCurrentAlternativeCountTo(usedIndexes);
        }

        public Interval GetTargetInterval(int index)
        {
            return Intervals.FirstOrDefault(interval =>
                !interval.IsFulfilled && interval.Limits.IsNumberWithinLimits(index));
        }

        private int Round(float value)
        {
            if ((int)(value + .5f) > (int)value)
                return (int)value + 1;
            return (int)value;
        }

        public class Interval
        {
            public Limits Limits { get; }
            public bool IsFulfilled { get; set; }

            public Interval(Limits limits)
            {
                Limits = limits;
            }
        }

        public readonly struct Limits
        {
            public int LowerLimit { get; }
            public int UpperLimit { get; }

            public Limits(int lowerLimit, int upperLimit)
            {
                LowerLimit = lowerLimit;
                UpperLimit = upperLimit;
            }

            public bool IsNumberWithinLimits(int value)
            {
                return value >= LowerLimit && value <= UpperLimit;
            }

            /// <param name="value">Current index</param>
            /// <param name="usedIndexes">The list of used indexes</param>
            /// <returns>Number of alternatives to given value within limits</returns>
            public int GetCurrentAlternativeCountTo(List<int> usedIndexes)
            {
                var alternativeCount = UpperLimit - LowerLimit + 1;
                for (int j = 0; j <= UpperLimit - LowerLimit; j++)
                {
                    var foundIndex = LowerLimit + j;
                    if (usedIndexes.Contains(foundIndex))
                        alternativeCount--;
                }

                return alternativeCount;
            }
        }
    }
}