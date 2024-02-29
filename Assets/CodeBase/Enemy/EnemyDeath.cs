using System;
using System.Collections;
using CodeBase.UI;
using CodeBase.UI.Elements;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyHealth), typeof(EnemyAnimator))]
    public class EnemyDeath : MonoBehaviour
    {
        public Collider HurtBox;
        public ActorUI ActorUI;
        public Attack Attack;
        public Aggro Aggro;
        public AgentMoveToPlayer MoveToPlayer;
        public EnemyHealth EnemyHealth;
        public EnemyAnimator EnemyAnimator;
        public GameObject DeathFX;
        public GameObject BloodPoolFX;

        public event Action Happened;

        private void Start() => 
            EnemyHealth.HealthChanged += OnHealthChanged;

        private void OnHealthChanged()
        {
            if (EnemyHealth.Current <= 0f) 
                Die();
        }

        private void Die()
        {
            HurtBox.enabled = false;
            EnemyHealth.HealthChanged -= OnHealthChanged;
            EnemyAnimator.PlayDeath();
            Attack.enabled = false;
            MoveToPlayer.enabled = false;
            Aggro.enabled = false;
            SpawnDeathFX();
            HideHpBar();
            StartCoroutine(DestroyTimer());
            Happened?.Invoke();
        }

        private void HideHpBar() => 
            ActorUI.HpBar.gameObject.SetActive(false);

        private void SpawnDeathFX()
        { 
            Instantiate(DeathFX, transform.position + Vector3.up, Quaternion.identity);
            Instantiate(BloodPoolFX, transform.position, Quaternion.identity);
        }

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }
    }
}