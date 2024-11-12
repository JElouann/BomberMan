using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    private PlayerInput _input;
    private Rigidbody2D _rb;
    private PlayerAnimation _animation;
    private InputAction _moveAction;

    private float _previousVelocityX;

    private void Start()
    {
        _input = TryGetComponent(out PlayerInput input) ? input : null;
        _rb = TryGetComponent(out Rigidbody2D rb) ? rb : null;
        _animation = TryGetComponent(out PlayerAnimation animation) ? animation : null;
        _moveAction = _input.actions.FindAction("Move");
    }

    private void FixedUpdate()
    {
        Vector2 dir = _moveAction.ReadValue<Vector2>();
        
        _rb.velocity = dir * _speed;

        if(dir.x != 0 && Mathf.Sign(dir.normalized.x) != Mathf.Sign(_previousVelocityX))
        {
            _previousVelocityX = Mathf.Sign(dir.normalized.x);
            _animation.FlipSpriteX();
        }        
    }
}
