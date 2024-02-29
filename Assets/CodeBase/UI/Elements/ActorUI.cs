using CodeBase.Hero;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class ActorUI : MonoBehaviour
    {
        public HpBar HpBar;

        private IHealth _health;

        private void Start()
        {
            IHealth health = GetComponent<IHealth>();

            if (health != null) 
                Construct(health);
        }

        public void Construct(IHealth health)
        {
            _health = health;
            health.HealthChanged += UpdateHpBar;
            if (_health is not HeroHealth)
                UpdateHpBar();
        }

        private void OnDestroy() => 
            _health.HealthChanged -= UpdateHpBar;
 
        private void UpdateHpBar() => 
            HpBar.SetValue(_health.Current, _health.Max);
    }
}