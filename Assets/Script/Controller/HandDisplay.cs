using System;
using UnityEngine;
using UnityEngine.Analytics;

namespace Script.Controller
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public class HandDisplay : MonoBehaviour
    {
        [Header("Sprites")]
        [SerializeField] private Sprite _hold;
        [SerializeField] private Sprite _idle;


        private Animator _tiredAnimation;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _tiredAnimation = GetComponent<Animator>();
        }

        public void SetSprite(HandSpriteState handSpriteState)
        {
            switch (handSpriteState)
            {
                case HandSpriteState.idle:
                    _spriteRenderer.sprite = _idle;
                    break;
                case HandSpriteState.hold:
                    _spriteRenderer.sprite = _hold;
                    break;
                
            }
        }

        public void StartTiredAnimation()
        {
            _tiredAnimation.enabled = true;
        }

        private void StopTiredAnimation()
        {
            _tiredAnimation.enabled = false;
        }

        public void SetAlpha(HandColor handColor)
        {
            StopTiredAnimation();
            float alpha = 0;
            switch (handColor)
            {
                case HandColor.normal:
                    alpha = 1;
                    break;
                case HandColor.dead:
                    alpha = 0.3f;
                    break;
            }

            _spriteRenderer.color = _spriteRenderer.color.With(a: alpha);
        }

    }
}