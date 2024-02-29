using CodeBase.Infrastructure.States;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Logic
{
    public class LevelTransferTrigger : MonoBehaviour
    {
        public string TransferTo;

        private const string PlayerTag = "Player";
        private bool _isTriggered;

        private IGameStateMachine _stateMachine;

        public void Awake()
        {
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isTriggered)
                return;
            
            if (other.CompareTag(PlayerTag))
            {
                _stateMachine.Enter<LoadLevelState, string>(TransferTo);
                _isTriggered = true;
            }
        }
    }
}