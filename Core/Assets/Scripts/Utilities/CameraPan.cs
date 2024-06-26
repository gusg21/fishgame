using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraPan : MonoBehaviour
{
    [SerializeField] private float panSpeed;

    private readonly Vector3 _defaultPosition = new Vector3(0f, 0f, -10f);
    private readonly Vector3 _fishingPosition = new Vector3(2f, 0f, -10f);
    
    private Vector3 _targetPosition = Vector3.zero;
    private bool _panning;

    private void Start()
    {
        SeymourController.onStartFishing += PanCamera;
        SeymourController.onStopFishing += ResetCamera;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, _targetPosition) <= 0.01)
        {
            _panning = false;
        }
        
        if (_panning)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, panSpeed * Time.deltaTime);
        }
    }

    private void PanCamera()
    {
        _targetPosition = _fishingPosition;
        _panning = true;
    }
    
    private void ResetCamera()
    {
        _targetPosition = _defaultPosition;
        _panning = true;
    }
}
