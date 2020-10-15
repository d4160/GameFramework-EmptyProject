using DG.Tweening;
using Game.Health;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Health
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private float _tweenDuration = .5f;
        [SerializeField] private Ease _easeType = Ease.InCubic;

        public void OnHPModified(int amount, int current, int max, Vector2 position)
        {
            var target = (float) current / max;

            DOTween.To(() => _slider.value, (v) => _slider.value = v, target, _tweenDuration).SetEase(_easeType);

            var manager = FlashTextUIManager.Instance as FlashTextUIManager;
            var flashText = manager?.AddObject();
            flashText?.Show(position, amount > 0 ? $"+{amount}" : $"{amount}", amount < 0 ? Color.red : Color.green);
        }

        public void RegisterEvent(HealthSystem healthSystem)
        {
            healthSystem.OnHPModified.DynamicCalls += OnHPModified;
        }
    }
}
