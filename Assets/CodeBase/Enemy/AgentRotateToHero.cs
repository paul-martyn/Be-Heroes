using UnityEngine;

namespace CodeBase.Enemy
{
    public class AgentRotateToHero : Agent
    {
        [SerializeField] private float _speed = 5f;

        private Transform _heroTransform;
        private Vector3 _positionToLook;

        public void Construct(Transform heroTransform) => 
            _heroTransform = heroTransform;
        
        private void Update() => 
            RotateTowardsHero();

        private void RotateTowardsHero()
        {
            UpdatePositionToLook();
            transform.rotation = SmoothedRotation(transform.rotation, _positionToLook);
        }

        private void UpdatePositionToLook()
        {
            Vector3 positionDiff = _heroTransform.position - transform.position;
            _positionToLook = new Vector3(positionDiff.x, transform.position.y, positionDiff.z);
        }

        private Quaternion SmoothedRotation(Quaternion rotation, Vector3 positionToLook) => 
            Quaternion.Lerp(rotation, TargetRotation(positionToLook), SpeedFactor());

        private Quaternion TargetRotation(Vector3 position) => 
            Quaternion.LookRotation(position);

        private float SpeedFactor() => 
            _speed * Time.deltaTime;

    }
}