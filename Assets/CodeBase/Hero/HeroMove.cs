using CodeBase.Data;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Hero
{
    public class HeroMove : MonoBehaviour, ISavedProgress
    {
        public CharacterController CharacterController;
        public HeroAnimator HeroAnimator;
        public float Speed = 1f;
        private IInputService _inputService;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _inputService = AllServices.Container.Single<IInputService>();
        }

        private void Update()
        {
            if (_inputService != null && !HeroAnimator.IsAttacking)
            {
                Vector3 movementVector = Vector3.zero;
                if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
                {
                    movementVector = _camera.transform.TransformDirection(_inputService.Axis);
                    movementVector.y = 0f;
                    movementVector.Normalize();
                    transform.forward = movementVector;
                }
                movementVector += Physics.gravity;
                CharacterController.Move(movementVector * (Speed * Time.deltaTime));
            }
        }

        public void LoadProgress(PlayerProgress playerProgress)
        {
            if (GetCurrentLevel() == playerProgress.WorldData.PositionOnLevel.Level)
            {
                Vector3Data savedPosition = playerProgress.WorldData.PositionOnLevel.Position;
                if (savedPosition != null)
                    Warp(to: savedPosition);
            }
        }

        public void UpdateProgress(PlayerProgress playerProgress)
        {
            playerProgress.WorldData.PositionOnLevel =
                new PositionOnLevel(transform.position.AsVectorData(), GetCurrentLevel());
        }

        private void Warp(Vector3Data to)
        {
            CharacterController.enabled = false;
            transform.position = to.AsUnityVector().AddY(CharacterController.height);
            CharacterController.enabled = true;
        }

        private static string GetCurrentLevel() =>
            SceneManager.GetActiveScene().name;
    }
}