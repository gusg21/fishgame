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
    [SerializeField] private FishingUI ui;
    [SerializeField] private List<DepthFishPool> depthFishPools;

    private Dictionary<Fish, int> _fishInventory = new();
    
    private List<Fish> _activeFish = new();
    private Fish _selectedFish;
    private LureType _selectedLure;
    private HookType _currentHookType;
    private int _currentDepth;

    private void Start()
    {
        SeymourController.onStartFishing += CreateActiveFish;
        
        _currentDepth = 0;
        
        ui.HideMinigame();
    }

    private void Update()
    {
        if (GameManager.CurrentMouseState == MouseState.FISHINGMINIGAME)
        {
            ui.CheckMinigameClick();
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
    }

    private Fish GetFishToCatch()
    {
        int chanceSum = 0;
        foreach (Fish fish in _activeFish)
        {
            foreach (LureType lure in fish.GetBestLure())
            {
                if (lure == _selectedLure)
                {
                    chanceSum += fish.GetCatchChance() + fish.GetBestLureCatchBonus();
                    break;
                }
            }
            chanceSum += fish.GetCatchChance();
            Debug.Log(chanceSum);
            break;
        }

        int luckyNumber = Random.Range(1, chanceSum);
        chanceSum = 0;
        
        foreach (Fish fish in _activeFish)
        {
            foreach (LureType lure in fish.GetBestLure())
            {
                if (lure == _selectedLure)
                {
                    if (luckyNumber > chanceSum && luckyNumber <= chanceSum + fish.GetCatchChance() + fish.GetBestLureCatchBonus())
                    {
                        return fish;
                    }

                    chanceSum += fish.GetCatchChance() + fish.GetBestLureCatchBonus();
                }
            }
            
            if (luckyNumber > chanceSum && luckyNumber <= chanceSum + fish.GetCatchChance())
            {
                return fish;
            }

            chanceSum += fish.GetCatchChance();
        }
        return null;
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
