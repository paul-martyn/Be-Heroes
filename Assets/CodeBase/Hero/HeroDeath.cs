using System;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Hero
{
    public class HeroDeath : MonoBehaviour
    {
        public HeroHealth HeroHealth;
        public HeroAttack HeroAttack;
        public HeroAnimator HeroAnimator;
        public HeroMove HeroMove;
        public GameObject DeathFX;

        public event Action Happened;

        private bool _isDead;
        private IWindowService _windowService;

        public void Construct(IWindowService windowService) => 
            _windowService = windowService;

        private void Start() => 
            HeroHealth.HealthChanged += OnHealthChanged;

        private void OnDestroy() => 
            HeroHealth.HealthChanged -= OnHealthChanged;

        private void OnHealthChanged()
        {
            if (HeroHealth.Current <= 0f && !_isDead) 
                Die();
        }

        private void Die()
        {
            _isDead = true;
            HeroMove.enabled = false;
            HeroAttack.enabled = false;
            HeroAnimator.PlayDeath();
            Instantiate(DeathFX, transform.position + Vector3.up, Quaternion.identity);
            Happened?.Invoke();
            _windowService.Open(WindowId.Death);
        }
    }
}