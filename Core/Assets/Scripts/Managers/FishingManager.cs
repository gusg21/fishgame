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
    
    // Minigame Settings
    private float _timeTillFishBite = 5f;
    private float _minigameFatigueReduceAmount = 0.05f;
    private float _minigamePlayerIconSpeed = 2f;
    private float _minigameFishBarSpeed = 2f;
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
        ui.UpdateMinigame(_currentFishFatigue);
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
        
        // This won't be here, will appear after fish move around
        ui.ShowMinigame();
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
    
    public void CheckMinigameClick()
    {
        if (ui.CheckMinigameZonesOverlap())
        {
            _currentFishFatigue -= _minigameFatigueReduceAmount;
            if (_currentFishFatigue <= 0)
            {
                _currentFishFatigue = 1f;
                CatchFish();
            }
        }
    }

    public void CatchFish()
    {
        Debug.Log("You caught a " + _selectedFish.GetName());
        AddFishToInventory(_selectedFish);
        ui.HideMinigame();
        _activeFish.Clear();
    }

    public void ReleaseFish()
    {
        Debug.Log("You failed to catch a " + _selectedFish.GetName());
        AddFishToInventory(_selectedFish);
        ui.HideMinigame();
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
