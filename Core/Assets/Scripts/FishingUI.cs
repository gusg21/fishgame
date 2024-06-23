using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingUI : MonoBehaviour
{
    [SerializeField] private GameObject fishingBackground;

    private void Start()
    {
        SeymourController.StartFishing += fishingBackground.GetComponent<MoveBetween>().MoveIn;
        SeymourController.StopFishing += fishingBackground.GetComponent<MoveBetween>().MoveOut;
    }
}
