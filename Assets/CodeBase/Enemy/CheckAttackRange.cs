using UnityEngine;

namespace CodeBase.Enemy
{
    public class CheckAttackRange : MonoBehaviour
    {
        [SerializeField] private Attack Attack;
        [SerializeField] private TriggerObserver TriggerObserver;

        private void Start()
        {
            Attack.DisableAttack();
            
            TriggerObserver.TriggerEnter += TriggerEnter;
            TriggerObserver.TriggerExit += TriggerExit;
        }

        private void TriggerExit(Collider obj) => 
            Attack.DisableAttack();

        private void TriggerEnter(Collider obj) => 
            Attack.EnableAttack();
    }
}