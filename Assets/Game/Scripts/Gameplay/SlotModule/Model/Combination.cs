using System;
using System.Linq;

namespace Gameplay.SlotModule.Model
{
    [Serializable]
    public struct Combination
    {
        public SlotObject.SlotObjectType[] SlotObjects;
        
        public bool DoesContainSameTypes()
        {
            var objectTypes = SlotObjects;
            return SlotObjects.All(o => o == objectTypes.First());
        }
    }
}