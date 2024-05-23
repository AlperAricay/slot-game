using UnityEngine;

namespace Gameplay.SlotModule.Model
{
    [CreateAssetMenu(fileName = "ProbabilityConfig", menuName = "Create Probability Config", order = 0)]
    public class ProbabilityConfig : ScriptableObject
    {
        public Probability[] Probabilities;
    }
}