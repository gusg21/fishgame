using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetween : MonoBehaviour
{
    [SerializeField] private bool _moveInOnAwake = false;
    [SerializeField] private bool _moveInOnEnable = false;
    [SerializeField] private bool _moveToOutPosOnAwake = false;
    [SerializeField] private float _moveTightness = 0.2f;
    [SerializeField] private float _moveSpeed = 2f;
    public Vector3 _posOut;
    public Vector3 _posIn;
    private Vector3 _currentTarget = Vector3.zero;
    private bool _moving;

    private void Awake()
    {
        if (_moveInOnAwake)
            MoveIn();

        if (_moveToOutPosOnAwake)
            transform.position = _posOut;
    }

    private void OnEnable()
    {
        if (_moveInOnEnable) 
            MoveIn();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.localPosition, _currentTarget) < 0.01f)
        {
            _moving = false;
        }
        if (_moving)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _currentTarget, _moveTightness * Time.deltaTime * _moveSpeed);
        }
    }

    private void Move(Vector3 from, Vector3 to)
    {
        transform.localPosition = from;
        _currentTarget = to;
        _moving = true;
    }

    public void MoveIn() => Move(_posOut, _posIn);
    public void MoveOut() => Move(_posIn, _posOut);

    public void MoveInRelative() => Move(_posOut + transform.localPosition, _posIn + transform.localPosition);
    public void MoveOutRelative() => Move(_posIn + transform.localPosition, _posOut + transform.localPosition);
    

}
