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

    [Header("Fish Zone")]
    [SerializeField] private Transform minigameFishZone;
    [SerializeField] private float fzScaleY;
    [SerializeField] private float fzMinY;
    [SerializeField] private float fzMaxY;
    
    [Header("Player Icon")]
    [SerializeField] private Transform minigamePlayerIcon;
    [SerializeField] private float piMinY;
    [SerializeField] private float piMaxY;
    
    [Header("Fatigue Bar")]
    [SerializeField] private SpriteProgressBar minigameFatigueBar;

    private void Start()
    {
        SeymourController.onStartFishing += SetDepthText;
        SeymourController.onStartFishing += fishingBackground.MoveIn;
        SeymourController.onStopFishing += fishingBackground.MoveOut;
    }

    public void UpdateMinigame(float fatigue)
    {
        // move icon
        // move fishbar
        // move fish
        minigameFatigueBar.SetValue(fatigue);
    }

    public bool CheckMinigameZonesOverlap()
    {
        if (Vector2.Distance(minigameFishZone.position, minigamePlayerIcon.position) <= fzScaleY / 2)
        {
            return true;
        }

        return false;
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
