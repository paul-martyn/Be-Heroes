using CodeBase.Infrastructure.Factory;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    public class AgentMoveToPlayer : Agent
    {
        [SerializeField] public NavMeshAgent _agent;

        private Transform _heroTransform;
        private IGameFactory _gameFactory;

        public void Construct(Transform heroTransform) => 
            _heroTransform = heroTransform;

        private void Update() => 
            SetDestinationForAgent();

        private void SetDestinationForAgent()
        {
            if (_heroTransform) 
                _agent.destination = _heroTransform.position;
        }
    }
}
