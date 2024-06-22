using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable All

public enum SeymourState
{
    // In UI or paused
    INACTIVE,
    // In-game and not acting
    IDLE,
    // Walking to location
    WALKING,
    // Fishing
    FISHING
}

public class SeymourController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private SeymourState _state;
    
    private Vector2 _targetPosition;
    private Vector2 _moveDirection;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // Stop moving if Seymour has reached target
        if (Vector2.Distance(transform.position, _targetPosition) <= .01)
        {
            _state = SeymourState.IDLE;
            _animator.SetBool("Walking", false);
        }
        
        // Keep walking if no inputs this frame
        if (_state == SeymourState.WALKING)
        {
            Vector2 transform2d = new Vector2(transform.position.x, transform.position.y);
            transform2d += _moveDirection * (walkSpeed * Time.deltaTime);
            transform.position = transform2d;
        }
        
        // Start moving if collider is clicked
        if (Input.GetMouseButtonDown(0))
        {
            switch (GameManager.CurrentMouseState)
            {
                case MouseState.DOCK:
                    Vector2 fixedMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    StartWalkingToPosition(fixedMousePos);
                    break;
                case MouseState.FISHING:
                    Debug.Log("You fished!");
                    break;
                
                default:
                    break;
            }
        }
    }

    public SeymourState GetSeymourState() => _state;
    
    private void StartWalkingToPosition(Vector2 target)
    {
        // Adjust by pivot
        target.y += .25f;
        
        _targetPosition = target;
        _moveDirection = (target - (Vector2)transform.position).normalized;

        if (_moveDirection.x < 0) _spriteRenderer.flipX = false;
        else if (_moveDirection.x >= 0) _spriteRenderer.flipX = true;

        _state = SeymourState.WALKING;
        _animator.SetBool("Walking", true);
    }
}
