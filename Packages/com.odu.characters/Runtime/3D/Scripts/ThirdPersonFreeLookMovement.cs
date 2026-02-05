using UnityEngine;
using UnityEngine.InputSystem;

namespace Odu.Characters.ThreeDimensional
{
    public class ThirdPersonFreeLookMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _visualTransform;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _speed;

        private Vector2 _direction;
        private ThirdPersonMovement inputActions;

        private void OnEnable()
        {
            inputActions = new ThirdPersonMovement();
            inputActions.Movement.Move.performed += InputPerform;
            inputActions.Movement.Move.canceled += InputCancel;

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Movement.Move.performed -= InputPerform;
            inputActions.Movement.Move.canceled -= InputCancel;

            inputActions.Disable();
        }

        private void InputPerform(InputAction.CallbackContext callbackContext)
        {
            MoveInputPerformed(callbackContext.ReadValue<Vector2>());
        }

        private void InputCancel(InputAction.CallbackContext callbackContext)
        {
            MoveInputCanceled();
        }

        private void MoveInputPerformed(Vector2 direction)
        {
            _direction = direction;
        }

        private void MoveInputCanceled()
        {
            _direction = Vector2.zero;
        }

        void FixedUpdate()
        {
            var inputDirection = new Vector3(_direction.x, 0, _direction.y);
            var direction = _cameraTransform.TransformDirection(inputDirection);
            direction.y = 0;

            if (Mathf.Abs(_direction.x) > 0 || Mathf.Abs(_direction.y) > 0)
            {
                var lookDirection = direction;
                lookDirection.y = 0;

                _visualTransform.forward = lookDirection;
            }

            _rigidbody.MovePosition(transform.position + (direction * _speed * Time.fixedDeltaTime));
        }
    }
}