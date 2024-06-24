using System;
using System.Collections;
using System.Collections.Generic;
using Agricosmic.Utilities;
using TMPro;
using UnityEngine;

public class FishingUI : MonoBehaviour
{
    [SerializeField] private MoveBetween fishingBackground;
    [SerializeField] private TextMeshPro depthText;

    [SerializeField] private BoxCollider2D minigameFishZone;
    [SerializeField] private BoxCollider2D minigamePlayerIcon;
    [SerializeField] private SpriteProgressBar minigameFatigueBar;

    private void Start()
    {
        SeymourController.onStartFishing += fishingBackground.MoveIn;
        SeymourController.onStartFishing += SetDepthText;
        SeymourController.onStopFishing += fishingBackground.MoveOut;
        SeymourController.onMinigameClick += CheckMinigameClick;
    }

    private void Update()
    {
        // move minigame objects
    }

    private void CheckMinigameClick()
    {
        if (Physics2D.IsTouching(minigameFishZone, minigamePlayerIcon))
        {
            // lower fish fatigue
            // check fish fatigue
            
        }
    }

    private void SetDepthText()
    {
        depthText.text = "Depth: " + GameManager.I.GetFishingManager().GetCurrentDepth() + "m";
    }
}
