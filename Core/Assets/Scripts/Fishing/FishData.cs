using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "Fishing/FishData")]
public class FishData : ScriptableObject
{
    [Header("Info")]
    public string Name;
    public Sprite Sprite;

    [Header("Fishing Data")]
    [Tooltip("1 (Rare) to 20 (Common)")]
    public int CatchChance;
    public List<LureType> BestLure;
    public int BestLureCatchBonus;

    [Header("Minigame Data")]
    [Tooltip("1 (Easy) to 5 (Hard)")]
    public int Difficulty;

    [Header("Commerce Data")] 
    public int SellPrice;
}
