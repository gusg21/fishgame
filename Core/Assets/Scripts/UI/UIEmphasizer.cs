using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIEmphasizer : MonoBehaviour
{
    private RectTransform _rectTransform;
    private float _timer;

    private void Start()
    {
        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (_timer > 0f)
        {
            _rectTransform.localScale = Vector3.one * (1f + _timer);
            var localRotation = _rectTransform.localRotation;
            localRotation.eulerAngles = Vector3.forward * Random.Range(-15f * _timer, 15f * _timer);
            _rectTransform.localRotation = localRotation;
            
            _timer -= Time.deltaTime;
        }
        else
        {
            _rectTransform.localRotation = Quaternion.identity;
            _rectTransform.localScale = Vector3.one;

            _timer = 0f;
        }
    }

    public void Emphasize(float time)
    {
        _timer = time;
    }
}
