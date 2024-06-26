using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public enum LureType
{
    NONE,
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
    NONE,
    BASICHOOK = 1,
    BETTERHOOK = 3,
    PROHOOK = 5,
    GOLDENHOOK = 7,
    DIAMONDHOOK = 9
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
    private List<Lure> _unlockedLures = new();
    private List<Fish> _activeFish = new();
    private Fish _selectedFish;
    private Lure _selectedLure;
    private int _currentLureIndex;
    private int _currentDepthIndex;
    private HookType _currentHookType;
    private DepthFishPool _currentDepth;
    
    // Timing Settings
    private bool _waitForFishBite;
    private float _timeTillFishBite = 5f;
    private bool _waitForFishRelease;
    private float _timeTillFishRelease = 15f;
    private float _currentTimer;
    
    // Minigame Settings
    private float _minigameFatigueChangeAmount = 0.2f;
    private float _currentFishFatigue = 1f;
    
    // Metagame Settings
    private bool _firstLureUnlocked = false;

    private void Start()
    {
        SeymourController.onStartFishing += CreateActiveFish;
        SeymourController.onMinigameClick += CheckMinigameClick;
        
        _currentHookType = SaveSystem.LoadHookType();
        List<LureType> unlockedLureSave = SaveSystem.LoadUnlockedLures();
        foreach (Lure lure in lures)
        {
            if (unlockedLureSave.Contains(lure.Type))
            {
                UnlockLure(lure);
                _firstLureUnlocked = true;
            }
        }
        _selectedLure = _firstLureUnlocked ? _unlockedLures[_currentDepthIndex] : ScriptableObject.CreateInstance<Lure>();
        
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
        if (GameManager.CurrentMouseState == MouseState.FISHINGMINIGAME)
        {
            if (_waitForFishRelease)
            {
                if (_currentTimer <= 0)
                {
                    ReleaseFish();
                    return;
                }
                
                ui.UpdateMinigame(_currentFishFatigue, (_timeTillFishRelease - _currentTimer) / _timeTillFishRelease);
                _currentTimer -= Time.deltaTime;
            }
            else if (_waitForFishBite)
            {
                if (_currentTimer <= 0)
                {
                    StartMinigame();
                }
                _currentTimer -= Time.deltaTime;
            }
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.CurrentMouseState == MouseState.FISHINGMINIGAME)
        {
            ui.UpdateFishVisuals();
        }
    }

    public int GetCurrentDepthLayer() => _currentDepth.DepthLayer;
    public int GetCurrentDepthInMeters() => _currentDepth.DepthInMeters;

    public bool GetFirstLureUnlocked() => _firstLureUnlocked;

    public List<FishData> GetBestFishByLure(LureType lure)
    {
        List<FishData> bestFish = new();
        foreach (DepthFishPool depth in depthFishPools)
        {
            foreach (FishData fish in depth.CatchableFish)
            {
                if (fish.BestLure.Contains(lure))
                {
                    bestFish.Add(fish);
                }
            }
        }

        return new();
    }

    public Lure GetNextUnlockedLure()
    {
        _currentLureIndex++;
        if (_currentLureIndex > _unlockedLures.Count - 1)
        {
            _currentLureIndex = 0;
        }
        
        _selectedLure = _unlockedLures[_currentLureIndex];
        
        return _selectedLure;
    }

    public Lure GetPreviousUnlockedLure()
    {
        _currentLureIndex--;
        if (_currentLureIndex < 0)
        {
            _currentLureIndex = _unlockedLures.Count - 1;
        }

        _selectedLure = _unlockedLures[_currentLureIndex];
        return _selectedLure;
    }
    
    public DepthFishPool GetNextDepth()
    {
        _currentDepthIndex++;
        if (_currentDepthIndex > depthFishPools.Count - 1)
        {
            _currentDepthIndex = depthFishPools.Count - 1;
        }

        if (_currentDepthIndex + 1 > (int)_currentHookType)
        {
            _currentDepthIndex = (int)_currentHookType - 1;
        }
        
        _currentDepth = depthFishPools[_currentDepthIndex];
        ui.SetDepthText();
        return _currentDepth;
    }

    public DepthFishPool GetPreviousDepth()
    {
        _currentDepthIndex--;
        if (_currentDepthIndex < 0)
        {
            _currentDepthIndex = 0;
        }
        
        _currentDepth = depthFishPools[_currentDepthIndex];
        ui.SetDepthText();
        return _currentDepth;
    }

    public void UnlockLure(Lure lureToUnlock)
    {
        if (_unlockedLures.Contains(lureToUnlock)) return;
        _unlockedLures.Add(lureToUnlock);
        _firstLureUnlocked = true;
    }
    
    public bool UnlockLure(LureType lureToUnlock)
    {
        if (lureToUnlock != LureType.NONE)
        {
            foreach (Lure lure in lures)
            {
                if (lureToUnlock == lure.Type)
                {
                    if (_unlockedLures.Contains(lure)) return false;
                    _unlockedLures.Add(lure); 
                    _firstLureUnlocked = true;
                    return true;
                }
            }
        }
        return false;
    }
    
    public bool UnlockHook(HookType hookToUnlock)
    {
        if (_currentHookType >= hookToUnlock) return false;
        _currentHookType = hookToUnlock;
        return true;
    }

    private void CreateActiveFish()
    {
        foreach (FishData data in _currentDepth.CatchableFish)
        {
            _activeFish.Add(new Fish(data));
        }
        _selectedFish = GetFishToCatch();
        
        ui.CreateFishVisuals(_activeFish, _selectedFish);
        
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
        ui.ClearFishVisuals();
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
