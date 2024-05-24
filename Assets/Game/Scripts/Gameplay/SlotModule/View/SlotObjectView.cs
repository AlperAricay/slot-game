using DG.Tweening;
using Gameplay.SlotModule.Model;
using UnityEngine;

namespace Gameplay.SlotModule.View
{
    public class SlotObjectView : MonoBehaviour
    {
        public bool IsSpinning { get; private set; }
        public int PosIndex { get; private set; }

        [field: SerializeField] private SpriteRenderer _spriteRenderer;

        private SlotObject _slotObject;
        private bool _isBlurred;
        private float[] _positions;

        public void Initialize(float[] positions, int posIndex)
        {
            _positions = positions;
            PosIndex = posIndex;
        }

        public void SetData(SlotObject slotObject)
        {
            _slotObject = slotObject;
            RefreshView();
        }

        public void ToggleBlur(bool value)
        {
            _isBlurred = value;
            RefreshView();
        }

        /// <summary>
        /// Advances slot view to the next position. Moves back to the top once fully advanced.
        /// </summary>
        /// <returns>False if the slot was already on the last index. Needs new data.</returns>
        public bool Advance(float duration, Ease ease = Ease.Linear)
        {
            IsSpinning = true;
            if (PosIndex >= _positions.Length - 1)
            {
                PosIndex = 0;
                var targetPos = transform.position;
                targetPos.y = _positions[PosIndex];
                transform.position = targetPos;
                IsSpinning = false;
                return false;
            }

            transform.DOMoveY(_positions[PosIndex + 1], duration)
                .SetEase(ease)
                .OnComplete(OnSlotAdvanced);
            return true;
        }

        private void OnSlotAdvanced()
        {
            PosIndex++;
            IsSpinning = false;
        }

        private void RefreshView()
        {
            _spriteRenderer.sprite = _isBlurred ? _slotObject.BlurredSprite : _slotObject.OriginalSprite;
        }
    }
}