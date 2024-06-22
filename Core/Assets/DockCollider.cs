using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockCollider : MonoBehaviour
{
    public static event Action<bool> MouseEnter;
    public static event Action<bool> MouseExit;

    private void OnMouseEnter()
    {
        MouseEnter?.Invoke(true);
    }
    
    private void OnMouseExit()
    {
        MouseExit?.Invoke(false);
    }
}