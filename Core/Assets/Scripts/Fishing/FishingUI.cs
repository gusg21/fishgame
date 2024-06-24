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
    
    private float _minigameFatigueReduceAmount = 0.05f;
    private float _minigamePlayerIconSpeed = 2f;
    private float _minigameFishBarSpeed = 2f;

    private void Start()
    {
        SeymourController.onStartFishing += fishingBackground.MoveIn;
        SeymourController.onStartFishing += SetDepthText;
        SeymourController.onStopFishing += fishingBackground.MoveOut;
        SeymourController.onMinigameClick += CheckMinigameClick;
    }

    public void UpdateMinigame()
    {
        // move icon
        // move fishbar
        // move fish
        // move fatigue
    }

    public void CheckMinigameClick()
    {
        if (Physics2D.IsTouching(minigameFishZone, minigamePlayerIcon))
        {
            // lower fish fatigue
            // check fish fatigue
            
        }
    }

    public void SetDepthText()
    {
        depthText.text = "Depth: " + GameManager.I.GetFishingManager().GetCurrentDepth() + "m";
    }

    public void ShowMinigame()
    {
        
    }

    public void HideMinigame()
    {
        
    }
}
