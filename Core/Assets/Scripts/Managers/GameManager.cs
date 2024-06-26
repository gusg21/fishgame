using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseState
{
    DEFAULT,
    UI,
    DOCK,
    FISHINGZONE,
    FISHINGMINIGAME,
}

public class GameManager : MonoBehaviour
{
    public static GameManager I;
    public static MouseState CurrentMouseState { get; set; }
    public float CurrentMoney { get; set; }

    private void Awake()
    {
        if (I == null) I = this;
        else Destroy(gameObject);
        
        DontDestroyOnLoad(this);

        CurrentMouseState = MouseState.DEFAULT;
    }

    private void OnDisable()
    {
        if (I == this) I = null;
    }
    
    [SerializeField] private SeymourController _seymour;
    [SerializeField] private FishingManager _fishingManager;
    [SerializeField] private HUDManager _hudManager;

    public SeymourController GetSeymour() => _seymour;
    public FishingManager GetFishingManager() => _fishingManager;
    public HUDManager GetHUDManager() => _hudManager;
}
