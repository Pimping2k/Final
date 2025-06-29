using System;
using System.Threading.Tasks;
using Configs;
using Core.Dependency;
using Core.DependencyInterfaces;
using Cysharp.Threading.Tasks;
using Gameplay.Managers;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player
{
    public class MovementComponent : MonoBehaviour
    {
        [SerializeField] private Transform _playerBody;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private CinemachineCamera _playerCamera;
        [SerializeField] private MovementConfig _config;
        [SerializeField] private StaminaComponent _staminaComponent;
        
        private IInputService _inputService;
        
        private bool _isSprinting;
        private bool _isCurrentlySprinting;
        private Vector2 _movementInput;
        private Vector2 _rotationInput;
        private float _xRotation;

        private void Awake()
        {
            _inputService = ServiceLocator.Resolve<IInputService>();
            
            _inputService.Player.Move.performed += OnMovePerformed;
            _inputService.Player.Move.canceled += OnMoveCanceled;
            _inputService.Player.Look.performed += OnLookPerformed;
            _inputService.Player.Look.canceled += OnLookCanceled;
            _inputService.Player.Sprint.performed += OnSprintPerformed;
            _inputService.Player.Sprint.canceled += OnSprintCanceled;
        }
        
        private void OnDestroy()
        {
            _inputService.Player.Move.performed -= OnMovePerformed;
            _inputService.Player.Move.canceled -= OnMoveCanceled;
            _inputService.Player.Look.performed -= OnLookPerformed;
            _inputService.Player.Look.canceled -= OnLookCanceled;
            _inputService.Player.Sprint.performed -= OnSprintPerformed;
            _inputService.Player.Sprint.canceled -= OnSprintCanceled;
        }

        private void FixedUpdate()
        {
            ApplyMovement();
            HandleSprinting();
        }

        private void LateUpdate()
        {
            ApplyCameraRotation();
        }

        #region Events

        private void OnMovePerformed(InputAction.CallbackContext obj)
        {
            _movementInput = obj.ReadValue<Vector2>();
        }

        private void OnMoveCanceled(InputAction.CallbackContext obj)
        {
            _movementInput = Vector2.zero;
        }

        private void OnLookPerformed(InputAction.CallbackContext obj)
        {
            _rotationInput = obj.ReadValue<Vector2>();
        }

        private void OnLookCanceled(InputAction.CallbackContext obj)
        {
            _rotationInput = Vector2.zero;
        }

        private async void OnSprintPerformed(InputAction.CallbackContext obj)
        {
            _isSprinting = true;
        }

        private void OnSprintCanceled(InputAction.CallbackContext obj)
        {
            _isSprinting = false;
        }

        #endregion

        #region Movement and Rotation calculation
        
        private void ApplyMovement()
        {
            Vector3 move = _playerBody.forward * _movementInput.y + _playerBody.right * _movementInput.x;

            Vector3 velocity = _isCurrentlySprinting
                ? move.normalized * _config.SprintSpeed
                : move.normalized * _config.MovementSpeed;
            
            
            _rigidbody.linearVelocity = new Vector3(velocity.x, _rigidbody.linearVelocity.y, velocity.z);
        }
        
        private async void HandleSprinting()
        {
            var isMoving = _rigidbody.linearVelocity.magnitude > 0.1f;
            
            if (_isSprinting && isMoving)
            {
                _staminaComponent.CancelReducingStamina();
                _staminaComponent.CancelRegenerationStamina();

                await _staminaComponent.TryReduceStamina(StaminaReduceType.Sprint);
                _isCurrentlySprinting = true;
            }
            else
            {
                _staminaComponent.CancelReducingStamina();
                _staminaComponent.RegenerateStamina();
                _isCurrentlySprinting = false;
            }
        }

        private void ApplyCameraRotation()
        {
            float mouseX = _rotationInput.x * _config.RotationSpeed * Time.fixedDeltaTime;
            float mouseY = _rotationInput.y * _config.RotationSpeed * Time.fixedDeltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -_config.CameraClampValue, _config.CameraClampValue);

            _playerCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            _playerBody.Rotate(Vector3.up * mouseX);
        }

        #endregion
    }
}