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
    FISHING,
}

public class SeymourController : MonoBehaviour
{
    public static event Action StartFishing;
    public static event Action StopFishing;
    
    [SerializeField] private float walkSpeed;

    [SerializeField] private SpriteRenderer _seymourRenderer;
    [SerializeField] private SpriteRenderer _fishingRodRenderer;
    [SerializeField] private Animator _seymourAnimator;
    [SerializeField] private Animator _fishingRodAnimator;

    private SeymourState _state;

    private Vector2 _fishingPos = new Vector2(.5f, -.15f);
    private bool _goingToFish;
    
    private Vector2 _targetPosition;
    private Vector2 _moveDirection;

    private void Update()
    {
        // Stop moving if Seymour has reached target
        if (Vector2.Distance(transform.position, _targetPosition) <= .01 && (_state == SeymourState.WALKING || _goingToFish))
        {
            if (_goingToFish)
            {
                transform.position = _targetPosition;
                
                _seymourRenderer.flipX = true;
                _fishingRodRenderer.flipX = true;
                
                StartFishing?.Invoke();
                _seymourAnimator.SetBool("Walking", false);
                _fishingRodAnimator.SetTrigger("Equip");
                _goingToFish = false;
            }
            else
            {
                transform.position = _targetPosition;
                _state = SeymourState.IDLE;
                _seymourAnimator.SetBool("Walking", false);
            }
        }
        
        // Keep walking if no inputs this frame
        if (_state == SeymourState.WALKING || _goingToFish)
        {
            Vector2 transform2d = new Vector2(transform.position.x, transform.position.y);
            transform2d += _moveDirection * (walkSpeed * Time.deltaTime);
            transform.position = transform2d;
        }
        
        // Start moving if collider is clicked
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 fixedMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            switch (GameManager.CurrentMouseState)
            {
                case MouseState.DOCK:
                    if (_state == SeymourState.FISHING)
                    {
                        _state = SeymourState.IDLE;
                        StopFishing?.Invoke();
                        _fishingRodAnimator.SetTrigger("Unequip");
                    }
                    else
                    {
                        StartWalkingToPosition(fixedMousePos);
                        _state = SeymourState.WALKING;
                    }
                    
                    break;
                case MouseState.FISHINGZONE:
                    if (_state == SeymourState.FISHING)
                    {
                        break;
                    }
                    WalkToFishingSpot(_fishingPos);
                    _state = SeymourState.FISHING;
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

        if (_moveDirection.x < 0) _seymourRenderer.flipX = false;
        else if (_moveDirection.x >= 0) _seymourRenderer.flipX = true;
        
        if (target.x < 0) _fishingRodRenderer.flipX = false;
        else if (target.x >= 0) _fishingRodRenderer.flipX = true;
        
        _seymourAnimator.SetBool("Walking", true);
    }

    private void WalkToFishingSpot(Vector2 target)
    {
        StartWalkingToPosition(target);
        _goingToFish = true;
    }
}
