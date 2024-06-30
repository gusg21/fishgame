using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishBoid : MonoBehaviour
{
    [SerializeField] private Vector2 boundsTL;
    [SerializeField] private Vector2 boundsBR;
    [SerializeField] private float boidSpeed;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Vector2 hookPosition;

    private Rigidbody2D _rb;
    private Vector2 _acceleration = Vector2.zero;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _rb.AddForce(GetAccelerationForce());

        _rb.velocity = _rb.velocity.normalized * boidSpeed;

        if (transform.position.x >= boundsBR.x || transform.position.x <= boundsTL.x)
        {
            _rb.velocity = new Vector2(-_rb.velocity.x, _rb.velocity.y);
            renderer.flipX = !renderer.flipX;
        }
        if (transform.position.y >= boundsTL.y || transform.position.y <= boundsBR.y)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, -_rb.velocity.y);
        }
    }

    private Vector2 GetAccelerationForce()
    {
        return new Vector2(_rb.velocity.x + Random.Range(-1f, 1f), _rb.velocity.y + Random.Range(-1f, 1f));
    }

    public void GoForHook()
    {
        
    }
}
