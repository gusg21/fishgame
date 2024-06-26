using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public enum LureType
{
    BAREHOOK,
    SUNBURSTPOPPER,
    EMERALDFROG,
    SILVERMINNOWCRANKBAIT,
    FIRECRACKERSPOON,
    COPPERCRAWFISHJERKBAIT,
    PINKSHIMMERWORM,
    GREENFLASHSPINNER,
    GOLDNUGGETCRANKBAIT,
    ELECTRICEELJERKBAIT,
    PURPLEHAZEDEEPDIVER,
    MIDNIGHTMARAUDERJIG,
    NEONGLOWSPOON,
    AQUAMIRAGEJIG,
    RUBYVORTEXTCRANKBAIT,
    RAINBOWSPARKLELURE,
    GLIMMERINGGOLDFISH,
    CRYSTALCLEARLURE,
    MYSTICMOONSPOON,
    HAUNTEDHOOK,
    PAPERMACHESPERMWHALE,
    BAGOFKRILL,
    COLOSSALSQUIDBAIT,
}

public enum HookType
{
    BASICHOOK = 0,
    BETTERHOOK = 5,
}

public class FishingManager : MonoBehaviour
{
    // Actions
    public static event Action onMinigameComplete;
    // References
    [SerializeField] private FishingUI ui;
    [SerializeField] private List<DepthFishPool> depthFishPools;
    [SerializeField] private List<Lure> lures;

    [Header("Fishing Popups")] 
    [SerializeField] private FishingPopup caughtPopup;
    [SerializeField] private FishingPopup releasedPopup;
    [SerializeField] private TextMeshPro popupMoneyText;
    [SerializeField] private TextMeshPro popupFishText;
    [SerializeField] private SpriteRenderer popupFishIcon;
    
    // Fishing Settings
    private Dictionary<Lure, bool> _unlockedLures = new();
    private List<Fish> _activeFish = new();
    private Fish _selectedFish;
    private Lure _selectedLure;
    private HookType _currentHookType;
    private int _currentDepth;
    
    // Timing Settings
    private bool _waitForFishBite;
    private float _timeTillFishBite = 5f;
    private bool _waitForFishRelease;
    private float _timeTillFishRelease = 15f;
    private float _currentTimer;
    
    // Minigame Settings
    private float _minigameFatigueChangeAmount = 0.2f;
    private float _currentFishFatigue = 1f;

    private void Start()
    {
        SeymourController.onStartFishing += CreateActiveFish;
        SeymourController.onMinigameClick += CheckMinigameClick;

        List<bool> unlockedLuresValues = SaveSystem.LoadUnlockedLures();
        for (int i = 0; i < lures.Count - 1; i++)
        {
            _unlockedLures.Add(lures[i], false);
        }
        
        _currentHookType = SaveSystem.LoadHookType();
        _currentDepth = 0;
        _selectedLure = lures[0];
        _unlockedLures[_selectedLure] = true;
        
        ui.HideMinigame();
    }

    private void OnDestroy()
    {
        SaveSystem.SaveHookType(_currentHookType);
        SaveSystem.SaveUnlockedLures(_unlockedLures);
        Debug.Log("Progress saved!");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            _currentHookType = HookType.BETTERHOOK;
        }
        if (GameManager.CurrentMouseState == MouseState.FISHINGMINIGAME)
        {
            if (_waitForFishRelease)
            {
                if (_currentTimer <= 0)
                {
                    ReleaseFish();
                    return;
                }
                
                ui.UpdateFishVisuals();
                ui.UpdateMinigame(_currentFishFatigue, (_timeTillFishRelease - _currentTimer) / _timeTillFishRelease);
                _currentTimer -= Time.deltaTime;
            }
            else if (_waitForFishBite)
            {
                if (_currentTimer <= 0)
                {
                    StartMinigame();
                }

                ui.UpdateFishVisuals();
                _currentTimer -= Time.deltaTime;
            }
        }
    }

    public int GetCurrentDepth() => _currentDepth;

    public Lure GetNextUnlockedLure()
    {
        return _selectedLure;
    }

    public Lure GetPreviousUnlockedLure()
    {
        return _selectedLure;
    }

    private void CreateActiveFish()
    {
        foreach (DepthFishPool pool in depthFishPools)
        {
            if (pool.DepthLayer == _currentDepth)
            {
                foreach (FishData data in pool.CatchableFish)
                {
                    _activeFish.Add(new Fish(data));
                }
                _selectedFish = GetFishToCatch();
                break;
            }
        }
        
        // Spawn visuals for fish
        
        GameManager.CurrentMouseState = MouseState.FISHINGMINIGAME;
        _currentTimer = _timeTillFishBite;
        _waitForFishBite = true;
    }

    private Fish GetFishToCatch()
    {
        int chanceSum = 0;
        foreach (Fish fish in _activeFish)
        {
            if (fish.GetBestLure().Contains(_selectedLure.Type))
            {
                chanceSum += fish.GetCatchChance() + fish.GetBestLureCatchBonus();
                continue;
            }
            chanceSum += fish.GetCatchChance();
        }

        int luckyNumber = Random.Range(1, chanceSum);
        chanceSum = 0;
        
        foreach (Fish fish in _activeFish)
        {
            if (fish.GetBestLure().Contains(_selectedLure.Type))
            {
                if (luckyNumber > chanceSum && luckyNumber <= chanceSum + fish.GetCatchChance() + fish.GetBestLureCatchBonus())
                {
                    return fish;
                }
                chanceSum += fish.GetCatchChance() + fish.GetBestLureCatchBonus();
                continue;
            }
            
            if (luckyNumber > chanceSum && luckyNumber <= chanceSum + fish.GetCatchChance())
            {
                return fish;
            }
            chanceSum += fish.GetCatchChance();
        }
        return null;
    }
    
    private void StartMinigame()
    {
        float fishZoneDifficulty = 0.6f - _selectedFish.GetDifficulty() * .1f;
        ui.ShowMinigame(fishZoneDifficulty);
        _currentFishFatigue = 1;
        _currentTimer = _timeTillFishRelease;
        _waitForFishBite = false;
        _waitForFishRelease = true;
    }

    private void StopMinigame()
    {
        ui.HideMinigame();
        GameManager.CurrentMouseState = MouseState.DEFAULT;
        _waitForFishRelease = false;
        onMinigameComplete?.Invoke();
    }
    
    public void CheckMinigameClick()
    {
        if (ui.CheckMinigameZonesOverlap())
        {
            _currentFishFatigue -= _minigameFatigueChangeAmount;
            if (_currentFishFatigue <= 0)
            {
                _currentFishFatigue = 1f;
                CatchFish();
            }
        }
        else
        {
            _currentFishFatigue += _minigameFatigueChangeAmount / 2;
            if (_currentFishFatigue > 1)
            {
                _currentFishFatigue = 1;
            }
        }
    }

    public void CatchFish()
    {
        popupFishText.text = "You caught a " + _selectedFish.GetName() + "!";
        popupMoneyText.text = _selectedFish.GetSellPrice().ToString();
        popupFishIcon.sprite = _selectedFish.GetSprite();
        caughtPopup.Appear();
        
        GameManager.I.CurrentMoney += _selectedFish.GetSellPrice();
        StopMinigame();
        _activeFish.Clear();
    }

    public void ReleaseFish()
    {
        releasedPopup.Appear();
        StopMinigame();
        _activeFish.Clear();
    }
}
