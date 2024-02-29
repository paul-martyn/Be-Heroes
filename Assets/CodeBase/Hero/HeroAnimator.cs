using System;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.Input;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Hero
{
    public class HeroAnimator : MonoBehaviour, IAnimationStateReader
    {
        [SerializeField] private Animator Animator;

        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;

        private IInputService _inputService;
        public AnimatorState State { get; private set; }

        private readonly int _idleHash = Animator.StringToHash("Idle");
        private readonly int _movingHash = Animator.StringToHash("IsMoving");
        private readonly int _attackHash1 = Animator.StringToHash("Attack_1");
        private readonly int _attackHash2 = Animator.StringToHash("Attack_2");
        private readonly int _hitHash = Animator.StringToHash("Hit");
        private readonly int _dieHash = Animator.StringToHash("Die");

        private void Awake() => 
            _inputService = AllServices.Container.Single<IInputService>();

        private void Update() =>
            RefreshMovingState();

        private void RefreshMovingState() =>
            Animator.SetBool(_movingHash, _inputService.Axis.magnitude > Constants.Epsilon);

        public bool IsAttacking =>
            State == AnimatorState.Attack;

        public void PlayHit() => 
            Animator.SetTrigger(_hitHash);

        public void PlayAttack()
        {
            int attackID = Random.Range(0, 2);
            switch (attackID)
            {
                case 0:
                    Animator.SetTrigger(_attackHash1);
                    return;
                case 1:
                    Animator.SetTrigger(_attackHash2);
                    return;
            }
        }

        public void PlayDeath() => 
            Animator.SetTrigger(_dieHash);

        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash) =>
            StateExited?.Invoke(StateFor(stateHash));

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;
            if (stateHash == _idleHash)
                state = AnimatorState.Idle;
            else if (stateHash == _movingHash)
                state = AnimatorState.Walking;
            else if (stateHash == _attackHash1 || stateHash == _attackHash2)
                state = AnimatorState.Attack;
            else if (stateHash == _dieHash)
                state = AnimatorState.Died;
            else
                state = AnimatorState.Unknown;

            return state;
        }
    }
}