using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum LureType
{
    NONE,
    TESTLURE,
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
    // For Carson to redo :)
    NONE,
    BASICHOOK = 2,
    BETTERHOOK = 5,
}

public class FishingManager : MonoBehaviour
{
    // Actions
    public static event Action onMinigameComplete;
    // References
    [SerializeField] private FishingUI ui;
    [SerializeField] private List<DepthFishPool> depthFishPools;

    // Inventory
    private Dictionary<Fish, int> _fishInventory = new();
    
    // Fishing Settings
    private List<Fish> _activeFish = new();
    private Fish _selectedFish;
    private LureType _selectedLure;
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
        
        _currentDepth = 0;
        
        ui.HideMinigame();
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
    
    private void SelectLure(LureType type) => _selectedLure = type;

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
            if (fish.GetBestLure().Contains(_selectedLure))
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
            if (fish.GetBestLure().Contains(_selectedLure))
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
        Debug.Log("You caught a " + _selectedFish.GetName());
        AddFishToInventory(_selectedFish);
        StopMinigame();
        _activeFish.Clear();
    }

    public void ReleaseFish()
    {
        Debug.Log("You failed to catch a " + _selectedFish.GetName());
        AddFishToInventory(_selectedFish);
        StopMinigame();
        _activeFish.Clear();
    }

    private void AddFishToInventory(Fish fishToAdd)
    {
        foreach (KeyValuePair<Fish, int> fish in _fishInventory)
        {
            if (fishToAdd == fish.Key) _fishInventory[fish.Key]++;
            return;
        }
        _fishInventory.Add(fishToAdd, 1);
    }
    
    private void RemoveFishFromInventory(Fish fishToRemove)
    {
        foreach (KeyValuePair<Fish, int> fish in _fishInventory)
        {
            if (fishToRemove == fish.Key) _fishInventory[fish.Key]--;
            if (_fishInventory[fish.Key] <= 0)
            {
                _fishInventory.Remove(fish.Key);
            }
            return;
        }
    }
    
}
