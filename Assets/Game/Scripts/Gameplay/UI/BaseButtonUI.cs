using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class BaseButtonUI : Button
    {
        private const float DownScaleMultiplier = 0.95f;
        private const float AnimationDuration = 0.08f;
        
        private Vector3 _initialButtonScale;

        public bool isAnimatedButton = true;
        public Transform targetScaleTransform;

        private Tween _playingAnimationTween;

        protected override void Start()
        {
            if (targetScaleTransform != null) 
                _initialButtonScale = targetScaleTransform.localScale;
            
            base.Start();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!isAnimatedButton || !IsInteractable())
                return;
            if (targetScaleTransform == null)
                return;
            
            Animate(_initialButtonScale * DownScaleMultiplier);
            base.OnPointerDown(eventData);
        }
        
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (!isAnimatedButton)
            {
                return;
            }
            
            if (targetScaleTransform == null)
            {
                return;
            }
            
            Animate(_initialButtonScale);
            base.OnPointerUp(eventData);
        }

        protected virtual void Animate(Vector3 scaleEndValue)
        {
            _playingAnimationTween?.Kill();
            _playingAnimationTween = targetScaleTransform.DOScale(scaleEndValue, AnimationDuration);
        }

        protected override void OnDestroy()
        {
            _playingAnimationTween?.Kill();
            base.OnDestroy();
        }
    }
}