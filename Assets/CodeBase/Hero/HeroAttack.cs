using CodeBase.Data;
using CodeBase.Logic;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator), typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour, ISavedProgressReader
    {
        public HeroAnimator HeroAnimator;
        public CharacterController CharacterController;
        public float AttackingDistance = 1f;

        private IInputService _inputService;
        private IRandomService _randomService;

        private int _attackingLayers;
        private const string AttackedLayerName = "Enemy";
        private readonly Collider[] _hits = new Collider[3];
        private Stats _heroStates;

        public void Construct(IInputService inputService, IRandomService randomService)
        {
            _inputService = inputService;
            _randomService = randomService;
        }

        private void Awake() => 
            SetAttackingLayers();

        private void Update()
        {
            if (_inputService != null && _inputService.IsAttackButtonPress() && !HeroAnimator.IsAttacking) 
                HeroAnimator.PlayAttack();
        }

        private void SetAttackingLayers() => 
            _attackingLayers = 1 << LayerMask.NameToLayer(layerName: AttackedLayerName);

        public void OnAttack()
        {
            for (int i = 0; i < Hit(); i++)
            {
                float damageValue = _randomService.Next(_heroStates.MinDamage, _heroStates.MaxDamage + 1);
                _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(damageValue);
            }
        }

        public void LoadProgress(PlayerProgress playerProgress) => 
            _heroStates = playerProgress.States;

        private int Hit() => 
            Physics.OverlapSphereNonAlloc(AttackPoint(), _heroStates.DamageRadius, _hits, _attackingLayers);

        private Vector3 AttackPoint() => 
            transform.position + (transform.forward * AttackingDistance) + new Vector3(0f, CharacterController.center.y / 2, 0f);
        
    }
}