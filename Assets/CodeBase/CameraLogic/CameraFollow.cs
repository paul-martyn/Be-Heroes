using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float _rotationAngleX = 0f;
        [SerializeField] private float _distance = 0f;
        [SerializeField] private float _offsetY = 0f;
        [Space(10f)]
        [SerializeField] private Transform _following;

        private void LateUpdate()
        {
            if (_following == null) return;

            var rotation = Quaternion.Euler(_rotationAngleX, 0f, 0f);
            Vector3 position = rotation * new Vector3(0f, 0f, -_distance) + FollowingPointPosition();

            transform.rotation = rotation;
            transform.position = position;
        }

        public void Follow(GameObject following) => 
            _following = following.transform;

        private Vector3 FollowingPointPosition()
        {
            Vector3 followingPosition = _following.position;
            followingPosition.y += _offsetY;
            return followingPosition;
        }
    }
}
