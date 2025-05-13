using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;

namespace LCPS.SlipForge.UI
{
    public class BossCanvasUI : MonoBehaviour
    {
        public RectTransform HealthBar;
        public Enemy.Enemy Boss;

        private float _maxHP;

        // Start is called before the first frame update
        void Start()
        {
            _maxHP = Boss.Health.Value;
            Boss.Health.OnValueChanged += OnHealthChange;

            StartCoroutine(AnimatedIntro());
        }

        private IEnumerator AnimatedIntro()
        {
            var start = Time.time;
            var end = start + 1.5f;
            while (Time.time < end)
            {
                // Scale the x axis from 0 to 1
                HealthBar.localScale = new Vector3((Time.time - start) / 1.5f, 1, 1);
                yield return null;
            }
        }

        private void OnHealthChange(float hp)
        {
            HealthBar.localScale = new Vector3(hp / _maxHP, 1, 1);
        }
        
        void OnDisable()
        {
            Boss.Health.OnValueChanged -= OnHealthChange;
        }
    }
}
