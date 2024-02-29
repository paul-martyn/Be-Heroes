using System.Linq;
using CodeBase.Hero;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]    
    public class Attack : MonoBehaviour
    {
        public float Cooldown;
        public float Cleavage;
        public float EffectiveDistance;
        public float Damage;

        [SerializeField] private EnemyAnimator Animator;
        [SerializeField] private EnemyAnimationCallbacks EnemyAnimationCallbacks;

        private Transform _heroTransform;
        private HeroDeath _heroDeath;
        private float _cooldown;
        private bool _isAttacking;
        private int _attackingLayers;
        private readonly Collider[] _hits = new Collider [1];
        private bool _attackIsActive;

        private const string AttackedLayerName = "Hero";

        public void Construct(Transform heroTransform, HeroDeath heroDeath)
        {
            _heroTransform = heroTransform;
            _heroDeath = heroDeath;

            _heroDeath.Happened += DisableAttack;
        }

        private void OnDestroy() => 
            _heroDeath.Happened -= DisableAttack;

        private void Awake()
        {
            SetAttackingLayers();
            EnemyAnimationCallbacks.Attack += OnAttack;
            EnemyAnimationCallbacks.AttackEnded += OnAttackEnded;
            EnemyAnimationCallbacks.GetHit += OnGetHit;
        }

        private void Update()
        {
            UpdateCooldown();
            
            if (CanAttack()) 
                StartAttack();
        }

        private void SetAttackingLayers() => 
            _attackingLayers = 1 << LayerMask.NameToLayer(layerName: AttackedLayerName);

        public void EnableAttack() => 
            _attackIsActive = true;

        public void DisableAttack() => 
            _attackIsActive = false;

        private void OnAttack()
        {
            PhysicsDebug.DrawDebug(AttackPoint(), Cleavage, 3f);
            if (Hit(out Collider hit)) 
                hit.transform.GetComponent<IHealth>().TakeDamage(Damage);
        }

        private void OnAttackEnded()
        {
            _isAttacking = false;
            _cooldown = Cooldown;
        }
        
        private void OnGetHit()
        {
            if (_isAttacking) 
                _cooldown = Cooldown;
            _isAttacking = false;
        }
        
        private bool CanAttack() => 
            !_isAttacking && IsCooldownUp() && _attackIsActive;

        private void StartAttack()
        {
            transform.LookAt(_heroTransform);
            Animator.PlayAttack();
            _isAttacking = true;
        }

        private bool Hit(out Collider hit)
        {
            int hitsCount = Physics.OverlapSphereNonAlloc(AttackPoint(), Cleavage, _hits, _attackingLayers);
            hit = _hits.FirstOrDefault();
            return hitsCount > 0;
        }

        private Vector3 AttackPoint() => 
            // ReSharper disable once Unity.InefficientPropertyAccess
            transform.position + transform.forward * EffectiveDistance + new Vector3(0f, 0.5f, 0f);

        private void UpdateCooldown()
        {
            if (!IsCooldownUp())
                _cooldown -= Time.deltaTime;
        }

        private bool  IsCooldownUp() => 
            _cooldown <= 0f;

    }
}
