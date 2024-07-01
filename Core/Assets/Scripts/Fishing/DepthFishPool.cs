using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fishing/DepthFishPool")]
public class DepthFishPool : ScriptableObject
{
    public int DepthLayer;
    public int DepthInMeters;
    public List<FishData> CatchableFish;
}
