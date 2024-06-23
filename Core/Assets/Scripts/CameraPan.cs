using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraPan : MonoBehaviour
{
    [SerializeField] private float panSpeed;

    private readonly Vector3 _defaultPosition = new Vector3(0f, 0f, -10f);
    private readonly Vector3 _fishingPositionLeft = new Vector3(-2f, 0f, -10f);
    private readonly Vector3 _fishingPositionRight = new Vector3(2f, 0f, -10f);
    
    private Vector3 _targetPosition = Vector3.zero;
    private bool _panning;

    private void Start()
    {
        SeymourController.StartFishing += PanCamera;
        SeymourController.StopFishing += ResetCamera;
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
        switch (GameManager.I.GetSeymour().GetSeymourState())
        {
            case SeymourState.FISHINGRIGHT:
                _targetPosition = _fishingPositionRight;
                break;
            case SeymourState.FISHINGLEFT:
                _targetPosition = _fishingPositionLeft;
                break;
        }
        _panning = true;
    }
    
    private void ResetCamera()
    {
        _targetPosition = _defaultPosition;
        _panning = true;
    }
}
