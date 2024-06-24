using System;
using System.Collections;
using System.Collections.Generic;
using Agricosmic.Utilities;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishingUI : MonoBehaviour
{
    [SerializeField] private MoveBetween fishingBackground;
    [SerializeField] private TextMeshPro depthText;
    [SerializeField] private SpriteRenderer minigameBarBackground;

    [Header("Fish Zone")]
    [SerializeField] private SpriteRenderer minigameFishZone;
    [SerializeField] private float fzScaleY;
    [SerializeField] private float fzHalfY;
    
    [Header("Player Icon")]
    [SerializeField] private SpriteRenderer minigamePlayerIcon;
    [SerializeField] private float piHalfY;
    
    [Header("Fatigue Bar")]
    [SerializeField] private SpriteProgressBar minigameFatigueBar;
    
    private float _minigamePlayerIconSpeed = 2f;
    private Vector3 _minigamePlayerIconDirection;
    private float _minigameFishZoneSpeed = 1f;
    private Vector3 _minigameFishZoneDirection;

    private void Start()
    {
        SeymourController.onStartFishing += SetDepthText;
        SeymourController.onStartFishing += fishingBackground.MoveIn;
        SeymourController.onStopFishing += fishingBackground.MoveOut;
    }

    public void UpdateMinigame(float fatigue)
    {
        // Move Player Icon
        if (minigamePlayerIcon.transform.position.y >= piHalfY || minigamePlayerIcon.transform.position.y <= -piHalfY)
        {
            _minigamePlayerIconDirection *= -1f;
            minigamePlayerIcon.transform.position += _minigamePlayerIconDirection * (_minigamePlayerIconSpeed * Time.deltaTime);
        }
        else
        {
            minigamePlayerIcon.transform.position += _minigamePlayerIconDirection * (_minigamePlayerIconSpeed * Time.deltaTime);
        }
        
        // Move Fish Zone
        if (minigameFishZone.transform.position.y >= fzHalfY || minigameFishZone.transform.position.y <= -fzHalfY)
        {
            _minigameFishZoneDirection *= -1f;
            _minigameFishZoneSpeed += Random.Range(-.05f, .05f);
            minigameFishZone.transform.position += _minigameFishZoneDirection * (_minigameFishZoneSpeed * Time.deltaTime);
        }
        else
        {
            _minigameFishZoneSpeed += Random.Range(-.05f, .05f);
            minigameFishZone.transform.position += _minigameFishZoneDirection * (_minigameFishZoneSpeed * Time.deltaTime);
        }
        
        // move fish visuals
        
        minigameFatigueBar.SetValue(fatigue);
    }

    public bool CheckMinigameZonesOverlap()
    {
        if (Vector2.Distance(minigameFishZone.transform.position, minigamePlayerIcon.transform.position) <= fzScaleY / 2)
        {
            return true;
        }

        return false;
    }

    public void SetDepthText()
    {
        depthText.text = "Depth: " + GameManager.I.GetFishingManager().GetCurrentDepth() + "m";
    }

    public void ShowMinigame(float fishZoneSize)
    {
        _minigamePlayerIconDirection = new Vector3(0, Random.Range(0,2)*2-1, 0);
        _minigameFishZoneDirection = new Vector3(0, Random.Range(0,2)*2-1, 0);
        
        minigameFishZone.transform.localScale = new Vector3(0.2f, fishZoneSize, 1f);
        fzHalfY = 1 + (0.05f * (6 - (fishZoneSize * 10f) - 1));
        
        minigameBarBackground.color = Color.grey;
        minigamePlayerIcon.color = Color.blue;
        minigameFishZone.color = Color.green;
    }

    public void HideMinigame()
    {
        minigamePlayerIcon.transform.position = new Vector3(minigamePlayerIcon.transform.position.x, 0, minigamePlayerIcon.transform.position.z);
        minigameFishZone.transform.position = new Vector3(minigameFishZone.transform.position.x, 0, minigameFishZone.transform.position.z);
        
        minigameFishZone.transform.localScale = new Vector3(0.2f, fzScaleY, 1f);
        fzHalfY = 1 + (0.05f * (6 - (fzScaleY * 10f) - 1));

        minigameBarBackground.color = Color.clear;
        minigamePlayerIcon.color = Color.clear;
        minigameFishZone.color = Color.clear;
    }
}
