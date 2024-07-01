using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishBoid : MonoBehaviour
{
    [SerializeField] private float boidSpeed = 1;
    [SerializeField] private SpriteRenderer renderer;
    
    private Rigidbody2D _rb;
    private Vector2 _acceleration = Vector2.zero;
    private Vector2 _hookPosition;
    private Vector2 _boundsTL;
    private Vector2 _boundsBR;
    public Fish FishType { get; set; }
    public bool GoingToHook { get; set; }
    public bool OnHook { get; set; }
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void UpdateMovement()
    {
        if (OnHook)
        {
            _rb.SetRotation(_rb.rotation + Random.Range(-10f, 10f));
        }
        else 
        {
            _rb.AddForce(GetAccelerationForce());

            _rb.velocity = _rb.velocity.normalized;

            if (transform.position.x >= _boundsBR.x || transform.position.x <= _boundsTL.x)
            {
                _rb.velocity = new Vector2(-_rb.velocity.x, _rb.velocity.y);
                renderer.flipX = !renderer.flipX;
            }
            if (transform.position.y >= _boundsTL.y || transform.position.y <= _boundsBR.y)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, -_rb.velocity.y);
            }
        }
    }

    private Vector2 GetAccelerationForce()
    {
        if (GoingToHook)
        {
            return (_hookPosition - (Vector2)transform.position).normalized;
        }
        return new Vector2(_rb.velocity.x + Random.Range(-1f, 1f), _rb.velocity.y + Random.Range(-1f, 1f));
    }

    public void SetTargetPosition(Vector2 pos)
    {
        _hookPosition = pos;
        GoingToHook = true;
    }

    public void SetBounds(float top, float bottom, float left, float right)
    {
        _boundsTL.y = top;
        _boundsBR.y = bottom;
        _boundsTL.x = left;
        _boundsBR.x = right;
    }

    public void SetSprite(Sprite sprite)
    {
        renderer.sprite = sprite;
    }
}
