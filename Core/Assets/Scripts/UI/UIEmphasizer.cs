using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIEmphasizer : MonoBehaviour
{
    private Transform _transform;
    private float _timer;

    private void Start()
    {
        if (_transform == null)
            _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (_timer > 0f)
        {
            _transform.localScale = Vector3.one * (1f + _timer);
            var localRotation = _transform.localRotation;
            localRotation.eulerAngles = Vector3.forward * Random.Range(-15f * _timer, 15f * _timer);
            _transform.localRotation = localRotation;
            
            _timer -= Time.deltaTime;
        }
        else
        {
            _transform.localRotation = Quaternion.identity;
            _transform.localScale = Vector3.one;

            _timer = 0f;
        }
    }

    public void Emphasize(float time)
    {
        _timer = time;
    }
}
