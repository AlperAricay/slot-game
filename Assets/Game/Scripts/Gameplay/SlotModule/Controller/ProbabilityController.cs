using System.Collections.Generic;
using System.Linq;
using Gameplay.SlotModule.Model;
using Gameplay.UserModule;
using Gameplay.Utility;

namespace Gameplay.SlotModule.Controller
{
    public class ProbabilityController
    {
        private readonly List<int> _usedIndexes = new();

        private readonly UserData _userData;
        private readonly ProbabilityConfig _probabilityConfig;

        public ProbabilityController(UserData userData, ProbabilityConfig probabilityConfig)
        {
            _userData = userData;
            _probabilityConfig = probabilityConfig;

            if (userData.SpinData == null) 
                GenerateSpinData();
        }

        public void GenerateSpinData()
        {
            _userData.SpinData = new Combination[100];
            var probabilities = _probabilityConfig.Probabilities.ToList();
            foreach (var probability in probabilities) probability.Initialize();
            
            for (int i = 0; i < 100; i++)
            {
                var mostRequiringProbability = probabilities[0];
                var minAlternativeCount = mostRequiringProbability.GetAlternativeCountToIndex(i, _usedIndexes);
                if (minAlternativeCount == -1) minAlternativeCount = 999;

                for (var j = 1; j < probabilities.Count; j++)
                {
                    var probability = probabilities[j];
                    var alternativeCount = probability.GetAlternativeCountToIndex(i, _usedIndexes);
                    if (alternativeCount == -1) continue;
                    if (alternativeCount >= minAlternativeCount) continue;

                    minAlternativeCount = alternativeCount;
                }

                var sameProbabilitiesList = new List<Probability>();
                for (var j = 0; j < probabilities.Count; j++)
                {
                    var probability = probabilities[j];
                    var alternativeCount = probability.GetAlternativeCountToIndex(i, _usedIndexes);
                    if (alternativeCount == -1) continue;
                    if (alternativeCount == minAlternativeCount)
                        sameProbabilitiesList.Add(probability);
                }
                mostRequiringProbability = sameProbabilitiesList.GetRandomElement();

                _userData.SpinData[i] = mostRequiringProbability.GetCombination();
                mostRequiringProbability.GetTargetInterval(i).IsFulfilled = true;
                _usedIndexes.Add(i);
            }
        }
    }
}