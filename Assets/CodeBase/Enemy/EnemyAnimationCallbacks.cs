using System;
using UnityEngine;

namespace CodeBase.Enemy
{    
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimationCallbacks : MonoBehaviour
    {
        public Action Attack;
        public Action AttackEnded;
        public Action GetHit;
        
        private void OnAttack() => 
            Attack.Invoke();

        private void OnAttackEnded() => 
            AttackEnded.Invoke();
        
        private void OnGetHit() => 
            GetHit.Invoke();
    }
}