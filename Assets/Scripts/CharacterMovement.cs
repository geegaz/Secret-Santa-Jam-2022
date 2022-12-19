using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Transform _playerVisual;
    [SerializeField] private Animator _playerAnimation;
    
    [Header("Move")]
    [SerializeField] private float _moveSpeed = 10.0f;
    [SerializeField, Range(0, 1)] private float _moveSpeedSmoothing = 0.05f;
    [SerializeField] private float _gravity = 10.0f;
    
    [SerializeField, ReadOnly] private Vector2 _moveDir;
    [SerializeField, ReadOnly] private Vector3 _velocity;

    [Header("Look")]
    [SerializeField] private Transform _lookObject;
    [SerializeField] private float _lookOffset;

    [SerializeField, ReadOnly] private Vector2 _lookDir;

    // [Header("Actions")]
    // [SerializeField] private InputActionProperty _movementAction;
    // [SerializeField] private InputActionProperty _attackAction;
    
    private CharacterController _characterController;
    private PlayerInput _playerInput;

    private void Awake() {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Update() {
        // --- Move ---
        Vector2 horizontal_vel = Vector2.zero;
        horizontal_vel.x = _velocity.x;
        horizontal_vel.y = _velocity.z;
        float vertical_vel = _velocity.y;

        if (_characterController.isGrounded) 
            vertical_vel = 0.0f;
        vertical_vel -= _gravity * Time.deltaTime;
        horizontal_vel = Vector2.Lerp(horizontal_vel, _moveDir * _moveSpeed, 1.0f - _moveSpeedSmoothing);
        
        _velocity.x = horizontal_vel.x;
        _velocity.y = vertical_vel;
        _velocity.z = horizontal_vel.y;

        _characterController.Move(_velocity * Time.deltaTime);

        // --- Look ---
        Vector3 horizontal_look = Vector3.zero;

        if (_lookDir.sqrMagnitude > 0.0f) {
            horizontal_look.x = _lookDir.x;
            horizontal_look.z = _lookDir.y;
        }
        else if (_moveDir.sqrMagnitude > 0.0f) {
            horizontal_look.x = _moveDir.x;
            horizontal_look.z = _moveDir.y;
            
        }
        
        if (horizontal_look.sqrMagnitude > 0.0f) {
            _lookObject.LookAt(transform.position + horizontal_look, Vector3.up);
            _lookObject.position = transform.position + horizontal_look * _lookOffset;
        }
    }

    private void OnMove(InputValue value) {
        _moveDir = value.Get<Vector2>();
    }

    private void OnLook(InputValue value) {
        _lookDir = value.Get<Vector2>();
    }
}
