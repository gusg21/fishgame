using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private SimpleButton storeButton;
    [SerializeField] private SimpleButton fishLibButton;
    [SerializeField] private SpriteRenderer moneyBackground;
    [SerializeField] private TextMeshPro moneyText;
    
    [Header("Lure HUD")]
    [SerializeField] private MoveBetween lureSelectMoveBetween;
    [SerializeField] private SpriteRenderer lureIcon;
    [SerializeField] private TextMeshPro lureNameText;
    [SerializeField] private SimpleButton nextButton;
    [SerializeField] private SimpleButton prevButton;
    
    private MoveBetween _storeButtonMoveBetween;
    private MoveBetween _fishLibButtonMoveBetween;
    private MoveBetween _moneyBGMoveBetween;

    private bool _storeOpen;
    private bool _libraryOpen;

    [Header("Store")] 
    [SerializeField] private MoveBetween storeMoveBetween;
    
    [Header("Fish Library")]
    [SerializeField] private MoveBetween libraryMoveBetween;

    private void Start()
    {
        SeymourController.onStartFishing += DisableHUD;
        SeymourController.onStopFishing += EnableHUD;
        
        _storeButtonMoveBetween = storeButton.GetComponent<MoveBetween>();
        _fishLibButtonMoveBetween = fishLibButton.GetComponent<MoveBetween>();
        _moneyBGMoveBetween = moneyBackground.GetComponent<MoveBetween>();
    }

    private void Update()
    {
        moneyText.text = "Money: " + GameManager.I.CurrentMoney;
    }

    public void ToggleStore()
    {
        if (!_libraryOpen)
        {
            _storeOpen = !_storeOpen;
            if (_storeOpen == true) storeMoveBetween.MoveIn();
            else storeMoveBetween.MoveOut();
        }
        else
        {
            _storeOpen = true;
            _libraryOpen = false;
            storeMoveBetween.MoveIn();
            libraryMoveBetween.MoveOut();
        }
    }
    
    public void ToggleFishLibrary()
    {
        if (!_storeOpen)
        {
            _libraryOpen = !_libraryOpen;
            if (_libraryOpen == true) libraryMoveBetween.MoveIn();
            else libraryMoveBetween.MoveOut();
        }
        else
        {
            _storeOpen = false;
            _libraryOpen = true;
            storeMoveBetween.MoveOut();
            libraryMoveBetween.MoveIn();
        }
    }

    public void SelectPreviousLure()
    {
        Lure nextLure = GameManager.I.GetFishingManager().GetPreviousUnlockedLure();
        lureIcon.sprite = nextLure.LureSprite;
        lureNameText.text = nextLure.name;
    }

    public void SelectNextLure()
    {
        Lure nextLure = GameManager.I.GetFishingManager().GetNextUnlockedLure();
        lureIcon.sprite = nextLure.LureSprite;
        lureNameText.text = nextLure.name;
    }
    
    public void EnableHUD()
    {
        storeButton.Active = true;
        fishLibButton.Active = true;
        nextButton.Active = true;
        prevButton.Active = true;
        
        _storeButtonMoveBetween.MoveIn();
        _fishLibButtonMoveBetween.MoveIn();
        _moneyBGMoveBetween.MoveIn();
        lureSelectMoveBetween.MoveIn();
    }

    public void DisableHUD()
    {
        storeButton.Active = false;
        fishLibButton.Active = false;
        nextButton.Active = false;
        prevButton.Active = false;
        
        _storeButtonMoveBetween.MoveOut();
        _fishLibButtonMoveBetween.MoveOut();
        _moneyBGMoveBetween.MoveOut();
        lureSelectMoveBetween.MoveOut();
    }
}
