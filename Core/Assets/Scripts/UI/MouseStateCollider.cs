using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MouseStateCollider : MonoBehaviour
{
    [SerializeField] private MouseState StateToSet;
    private void OnMouseEnter()
    {
        if (GameManager.CurrentMouseState == MouseState.DEFAULT && GameManager.CurrentMouseState != MouseState.UI)
            GameManager.CurrentMouseState = StateToSet;
    }
    
    private void OnMouseExit()
    {
        if (GameManager.CurrentMouseState != MouseState.FISHINGMINIGAME && GameManager.CurrentMouseState != MouseState.UI)
            GameManager.CurrentMouseState = MouseState.DEFAULT;
    }
}