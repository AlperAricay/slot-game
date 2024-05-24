using UnityEngine;

namespace Gameplay.SlotModule.Model
{
    [CreateAssetMenu(fileName = "Slot_Object", menuName = "Create Slot Object", order = 0)]
    public class SlotObject : ScriptableObject
    {
        public SlotObjectType Type;
        public int PrizeValue;
        public Sprite BlurredSprite;
        public Sprite OriginalSprite;
        
        public enum SlotObjectType
        {
            A = 0,
            Bonus,
            Seven,
            Wild,
            Jackpot
        }
    }
}