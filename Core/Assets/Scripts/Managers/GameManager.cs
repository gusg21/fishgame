using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseState
{
    DEFAULT,
    UI,
    DOCK,
    FISHINGZONELEFT,
    FISHINGZONERIGHT,
    FISHINGMINIGAME,
}

public class GameManager : MonoBehaviour
{
    public static GameManager I;
    public static MouseState CurrentMouseState { get; set; }

    private void Awake()
    {
        if (I == null) I = this;
        else Destroy(gameObject);
        
        DontDestroyOnLoad(this);
    }

    private void OnDisable()
    {
        if (I == this) I = null;
    }
    
    [SerializeField] private SeymourController _seymour;

    public SeymourController GetSeymour() => _seymour;
}
