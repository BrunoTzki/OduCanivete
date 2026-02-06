using UnityEngine;

namespace OduLib.Systems.Characters.ThreeDimensional
{
    public class ThirdPersonTopVisionMovement : CharacterMovement
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _visualTransform;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _speed;

        public override void Enter()
        {
            base.Enter();

            enabled = true;
        }

        public override void Exit()
        {
            base.Exit();

            enabled = false;
        }

        private void FixedUpdate()
        {
            MoveAndLook();
        }

        private void MoveAndLook()
        {
            Vector3 direction = CalculateDirection();

            TryChangeLookDirection(direction);

            _rigidbody.MovePosition(transform.position + (direction * _speed * Time.fixedDeltaTime));
        }

        private void TryChangeLookDirection(Vector3 direction)
        {
            if (Mathf.Abs(_movementDirection.x) > 0 || Mathf.Abs(_movementDirection.y) > 0)
            {
                var lookDirection = direction;
                lookDirection.y = 0;

                _visualTransform.forward = lookDirection;
            }
        }

        private Vector3 CalculateDirection()
        {
            var inputDirection = new Vector3(_movementDirection.x, 0, _movementDirection.y);
            var direction = _cameraTransform.TransformDirection(inputDirection);
            direction.y = 0;
            return direction;
        }
    }
}
