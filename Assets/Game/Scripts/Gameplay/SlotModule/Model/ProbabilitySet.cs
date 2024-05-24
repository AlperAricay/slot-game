using System;
using System.Linq;

namespace Gameplay.SlotModule.Model
{
    [Serializable]
    public class ProbabilitySet
    {
        public SlotObject[] SlotObjects;

        public SlotObject GetSlotObjectByType(SlotObject.SlotObjectType slotObjectType)
        {
            return SlotObjects.First(slotObject => slotObject.Type == slotObjectType);
        }
    }
}
