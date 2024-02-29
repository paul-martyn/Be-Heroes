using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        public EnemyAnimator EnemyAnimator;
        public GameObject HitFX;

        [SerializeField] private float _current;
        [SerializeField] private float _max;

        public event Action HealthChanged;

        public float Max
        {
            get => _max;
            set => _max = value;
        }

        public float Current
        {
            get => _current;
            set => _current = value;
        }

        public void TakeDamage(float damageValue)
        {
            SpawnHitFX();
            Current -= damageValue;
            EnemyAnimator.PlayHit();
            HealthChanged?.Invoke();
        }
        
        private void SpawnHitFX() => 
            Instantiate(HitFX, transform.position + Vector3.up, Quaternion.identity);
    }
}