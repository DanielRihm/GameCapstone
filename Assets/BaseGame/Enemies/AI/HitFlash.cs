using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace LCPS.SlipForge.Enemy.AI
{
    public class HitFlash : MonoBehaviour
    {
        public float HitFlashTime = 0.1f;
        public Color FlashColor = Color.red;

        private SpriteRenderer _spriteRenderer;
        private Enemy _self;
        private Color _spriteColor;

        private void OnEnable()
        {
            if(_spriteRenderer == null)
                _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            if(_self == null)
                _self = GetComponent<Enemy>();
            
            Assert.IsNotNull(_self, $"{this.name} requires an Enemy component.");
            Assert.IsNotNull(_spriteRenderer, $"{this.name} requires an Sprite Renderer component.");

            _spriteColor = _spriteRenderer.color;
            _currentColor = _spriteColor;
            _self.Health.OnValueChanged += OnHit;
        }

        private void OnDisable()
        {
            _self.Health.OnValueChanged -= OnHit;
            _spriteRenderer.color = _spriteColor;
        }

        private void OnHit(float hp)
        {
            StartCoroutine(PreformHitFlash());
        }

        private Color _currentColor;
        
        private IEnumerator PreformHitFlash()
        {

            var start = Time.time;
            _currentColor = FlashColor;
            while(Time.time - start < HitFlashTime)
            {
                _currentColor = Color.Lerp(FlashColor, _spriteColor, (Time.time - start) / HitFlashTime);
                yield return null;
            }
        }

        private void LateUpdate()
        {
            if(_spriteRenderer.color != _currentColor)
            {
                _spriteRenderer.color = _currentColor;
            }
        }
    }
}
