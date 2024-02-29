using System;
using CodeBase.Hero;
using CodeBase.Logic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Enemy
{
    public class EnemyAnimator : MonoBehaviour, IAnimationStateReader
    {
        [SerializeField] private Animator Animator;
        
        private static readonly int Attack1 = Animator.StringToHash("Attack_1");
        private static readonly int Attack2 = Animator.StringToHash("Attack_2");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int Win = Animator.StringToHash("Win");
        
        private static readonly int IdleStateHash = Animator.StringToHash("Idle");
        private static readonly int AttackStateHash = Animator.StringToHash("attack01");
        private static readonly int WalkingStateHash = Animator.StringToHash("Move");
        private static readonly int DeathStateHash = Animator.StringToHash("die");

        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;
        
        private HeroDeath _heroDeath;

        public void Construct(HeroDeath heroDeath)
        {
            _heroDeath = heroDeath;
            _heroDeath.Happened += PlayWin;
        }

        private void OnDestroy()
        {
            _heroDeath.Happened -= PlayWin;
        }

        public void Move(float speed)
        {
            Animator.SetBool(IsMoving, true);
            Animator.SetFloat(Speed, speed);
        }

        public void PlayHit() => 
            Animator.SetTrigger(Hit);

        public void PlayDeath()
        {
            Animator.ResetTrigger(Attack1);
            Animator.ResetTrigger(Attack2);
            Animator.ResetTrigger(Hit);
            Animator.SetTrigger(Die);
        }

        public void StopMoving() => 
            Animator.SetBool(IsMoving, false);

        private void PlayWin() => 
            Animator.SetTrigger(Win);

        public void PlayAttack()
        {
            int attackID = Random.Range(0, 2);
            switch (attackID)
            {
                case 0:
                    Animator.SetTrigger(Attack1);
                    return;
                case 1:
                    Animator.SetTrigger(Attack2);
                    return;
            }
        }

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state = AnimatorState.Unknown;
            if (stateHash == IdleStateHash)
                state = AnimatorState.Idle;
            else if (stateHash == AttackStateHash)
                state = AnimatorState.Attack;
            else if (stateHash == WalkingStateHash)
                state = AnimatorState.Walking;
            else if (stateHash == DeathStateHash) 
                state = AnimatorState.Died;
            return state;
        }

        #region IAnimatorStateReader

        public AnimatorState State { get; set; }

        public void EnteredState(int stateHash)
        {
            if (State == AnimatorState.Died)
                return;
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash) => 
            StateExited?.Invoke(State);

        #endregion
    }
}
