using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish
{
    private FishData _data;
    
    public string GetName() => _data.Name;
    public Sprite GetSprite() => _data.Sprite;
    public int GetCatchChance() => _data.CatchChance;
    public List<LureType> GetBestLure() => _data.BestLure;
    public int GetBestLureCatchBonus() => _data.BestLureCatchBonus;
    public int GetDifficulty() => _data.Difficulty;
    public float GetSellPrice() => _data.SellPrice;

    public Fish(FishData data) => _data = data;
}
