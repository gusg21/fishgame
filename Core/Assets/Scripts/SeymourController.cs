using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable All

public class SeymourController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    
    private Vector2 _targetPosition;
    private Vector2 _moveDirection;
    private bool _walking;

    private bool _mouseOverDock;

    private void Awake()
    {
        DockCollider.MouseEnter += SetMouseOverDock;
        DockCollider.MouseExit += SetMouseOverDock;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // Stop moving if Seymour has reached target
        if (Vector2.Distance(transform.position, _targetPosition) <= .01)
        {
            _walking = false;
            _animator.SetBool("Walking", false);
        }
        
        // Keep walking if no inputs this frame
        if (_walking)
        {
            Vector2 transform2d = new Vector2(transform.position.x, transform.position.y);
            transform2d += _moveDirection * (walkSpeed * Time.deltaTime);
            transform.position = transform2d;
        }
        
        // Start moving if collider is clicked
        if (Input.GetMouseButtonDown(0) && _mouseOverDock)
        {
            Vector2 fixedMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            StartWalkingToPosition(fixedMousePos);
        }
    }

    private void SetMouseOverDock(bool value) => _mouseOverDock = value;

    private void StartWalkingToPosition(Vector2 target)
    {
        // Adjust by pivot
        target.y += .25f;
        
        _targetPosition = target;
        _moveDirection = (target - (Vector2)transform.position).normalized;

        if (_moveDirection.x < 0) _spriteRenderer.flipX = false;
        else if (_moveDirection.x >= 0) _spriteRenderer.flipX = true;
        
        _walking = true;
        _animator.SetBool("Walking", true);
    }
}
