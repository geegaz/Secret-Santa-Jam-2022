using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10.0f;
    [SerializeField, Range(0, 1)] private float _moveSpeedSmoothing = 0.05f;
    [SerializeField] private float _gravity = 10.0f;

    // [Header("Actions")]
    // [SerializeField] private InputActionProperty _movementAction;
    // [SerializeField] private InputActionProperty _attackAction;

    private Vector2 _dir;
    private Vector3 _velocity;
    
    private CharacterController _characterController;
    private PlayerInput _playerInput;

    private void Awake() {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Update() {
        Vector2 horizontal_vel = Vector2.zero;
        horizontal_vel.x = _velocity.x;
        horizontal_vel.y = _velocity.z;
        float vertical_vel = _velocity.y;

        if (_characterController.isGrounded) 
            vertical_vel = 0.0f;
        vertical_vel -= _gravity * Time.deltaTime;
        horizontal_vel = Vector2.Lerp(horizontal_vel, _dir * _moveSpeed, 1.0f - _moveSpeedSmoothing);
        
        _velocity.x = horizontal_vel.x;
        _velocity.y = vertical_vel;
        _velocity.z = horizontal_vel.y;

        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void OnMove(InputValue value) {
        _dir = value.Get<Vector2>();
        Debug.Log("Move: "+_dir);
    }

    private void OnFire(InputValue value) {
        Debug.Log("Fire: " + value.isPressed);
    }
}
