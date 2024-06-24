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
    [Tooltip(
        "Put this on a scale between 1 and 20, from 1 being very rare to 20 being common")]
    public int CatchChance;
    public LureType BestLure;
    public int BestLureCatchBonus;

    [Header("Commerce Data")] 
    public int SellPrice;
}
