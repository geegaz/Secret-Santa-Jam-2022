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
    [SerializeField, Range(0, 1)] private float _moveSpeedSmoothing = 0.8f;
    [SerializeField] private float _gravity = 10.0f;
    
    [SerializeField, ReadOnly] private Vector2 _moveDir;
    [SerializeField, ReadOnly] private Vector3 _velocity;

    [Header("Look")]
    [SerializeField] private float _lookOffset;
    [SerializeField, Range(0, 1)] private float _lookSmoothing = 0.8f;

    [SerializeField, ReadOnly] private Vector2 _lookDir;
    [SerializeField, ReadOnly] private Vector3 _direction;

    [Header("Attack")]
    [SerializeField] private CharacterAttack _attackObject;

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
        Vector2 horizontal_dir = Vector2.zero;
        horizontal_dir.x = _direction.x;
        horizontal_dir.y = _direction.z;

        if (_lookDir.sqrMagnitude > 0.1f) 
            horizontal_dir = Vector2.Lerp(horizontal_dir, _lookDir, 1.0f - _lookSmoothing).normalized;
        else if (_moveDir.sqrMagnitude > 0.1f) 
            horizontal_dir = Vector2.Lerp(horizontal_dir, _moveDir, 1.0f - _lookSmoothing).normalized;
        _direction.x = horizontal_dir.x;
        _direction.z = horizontal_dir.y;

        if (_attackObject != null)
            _attackObject.transform.position = transform.position + _direction * _lookOffset;

        // --- Animation ---
        float dot_dir = Vector2.Dot(horizontal_dir.normalized, horizontal_vel.normalized);
        bool running = _moveDir.magnitude > 0.1f;
        if (running)
            _playerVisual.LookAt(transform.position + _direction, Vector3.up);
        _playerAnimation.SetBool("Running", running);
        _playerAnimation.SetFloat("Speed", (horizontal_vel.magnitude / _moveSpeed) * dot_dir);
    }

    private void OnMove(InputValue value) {
        _moveDir = value.Get<Vector2>();
    }

    private void OnLook(InputValue value) {
        _lookDir = value.Get<Vector2>();
    }

    private void OnFire(InputValue value) {
        // TODO: Call attack function on the _attackObject
        _playerAnimation.SetTrigger("Attack");
    }

    private void OnDrawGizmos() {
        Vector3 dir_pos = transform.position + _direction * _lookOffset;
        Gizmos.DrawLine(transform.position, dir_pos);
        Gizmos.DrawWireSphere(dir_pos, 0.25f);
    }
}
