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
    GLIMMERINGGOLDFISH,
    CRYSTALCLEARLURE,
    MYSTICMOONSPOON,
    HAUNTEDHOOK,
    PAPERMACHESPERMWHALE,
    BAGOFKRILL,
    COLOSSALSQUIDBAIT,

}

public class FishingManager : MonoBehaviour
{
    [SerializeField] private List<DepthFishPool> depthFishPools;

    private Dictionary<Fish, int> _fishInventory = new();
    
    private List<Fish> _activeFish = new();
    private Fish _selectedFish;
    private LureType _lureSelected;
    private int _currentDepth;

    private void Start()
    {
        SeymourController.onStartFishing += CreateActiveFish;
        _currentDepth = 0;
    }

    private void Update()
    {
        
    }

    public int GetCurrentDepth() => _currentDepth;
    
    private void SelectLure(LureType type) => _lureSelected = type;

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
                return;
            }
        }
    }

    private Fish GetFishToCatch()
    {
        int chanceSum = 0;
        foreach (Fish fish in _activeFish)
        {
            if (fish.GetBestLure() == _lureSelected)
            {
                chanceSum += fish.GetCatchChance() + fish.GetBestLureCatchBonus();
            }
            else
            {
                chanceSum += fish.GetCatchChance();
            }
        }

        int luckyNumber = Random.Range(1, chanceSum);
        chanceSum = 0;
        
        foreach (Fish fish in _activeFish)
        {
            if (fish.GetBestLure() == _lureSelected)
            {
                if (luckyNumber > chanceSum && luckyNumber <= chanceSum + fish.GetCatchChance() + fish.GetBestLureCatchBonus())
                {
                    return fish;
                }

                chanceSum += fish.GetCatchChance() + fish.GetBestLureCatchBonus();
            }
            else
            {
                if (luckyNumber > chanceSum && luckyNumber <= chanceSum + fish.GetCatchChance())
                {
                    return fish;
                }

                chanceSum += fish.GetCatchChance();
            }
        }
        return null;
    }

    public void CatchFish()
    {
        Debug.Log("You caught a " + _selectedFish.GetName());
        AddFishToInventory(_selectedFish);
        _activeFish.Clear();
    }

    public void ReleaseFish()
    {
        Debug.Log("You failed to catch a " + _selectedFish.GetName());
        AddFishToInventory(_selectedFish);
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
