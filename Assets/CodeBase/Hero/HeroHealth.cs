using System;
using CodeBase.Data;
using CodeBase.Logic;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, IHealth, ISavedProgress
    {
        public HeroAnimator Animator;

        public event Action HealthChanged;

        private State _state;

        public float Current
        {
            get => _state.CurrentHp;
            set
            {
                if (Math.Abs(_state.CurrentHp - value) > Constants.Epsilon)
                {
                    _state.CurrentHp = Math.Clamp(value, 0f, _state.MaxHp);
                    HealthChanged?.Invoke();
                }
            }
        }
        public float Max
        {
            get => _state.MaxHp;
            set => _state.MaxHp = value;
        }

        public void LoadProgress(PlayerProgress playerProgress)
        {
            _state = playerProgress.HeroState;
            HealthChanged?.Invoke();
        }

        public void UpdateProgress(PlayerProgress playerProgress)
        {
            playerProgress.HeroState.CurrentHp = Current;
            playerProgress.HeroState.MaxHp = Max;
        }

        public void TakeDamage(float damageValue)
        {
            if (Current <= 0f)
                return;

            Animator.PlayHit();
            Current -= damageValue;
        }
    }
}