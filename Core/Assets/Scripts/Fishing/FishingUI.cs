using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingUI : MonoBehaviour
{
    [SerializeField] private MoveBetween fishingBackground;

    private void Start()
    {
        SeymourController.StartFishing += fishingBackground.MoveIn;
        SeymourController.StopFishing += fishingBackground.MoveOut;
    }
}
