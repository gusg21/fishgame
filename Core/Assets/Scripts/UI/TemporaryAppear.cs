using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class TemporaryAppear : MonoBehaviour
{
    [SerializeField] private float fadeInTime;
    [SerializeField] private float lingerTime;
    [SerializeField] private float fadeOutTime;

    private float _timer;
    private bool _active;

    private void Start()
    {
        _timer = 0f;
    }

    private void Update()
    {
        if (_active)
        {
            if (_timer >= fadeInTime + lingerTime + fadeOutTime)
            {
                _active = false;
                return;
            }

            if (_timer >= 0f && _timer < fadeInTime)
            {
                // Fade in
            }
            else if (_timer >= fadeInTime && _timer < lingerTime + fadeInTime)
            {
                // Linger
            }
            else if (_timer >= lingerTime + fadeInTime)
            {
                // Fade out
            }
            
            _timer += Time.deltaTime;
        }
    }

    public void Appear()
    {
        _timer = 0f;
        _active = true;
    }
}
