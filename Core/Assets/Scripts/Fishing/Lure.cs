using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fishing/Lure")]
public class Lure : ScriptableObject
{
    public LureType Type;
    public Sprite LureSprite;
}
