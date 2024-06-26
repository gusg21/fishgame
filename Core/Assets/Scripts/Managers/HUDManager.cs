using System;
using System.Collections;
using System.Collections.Generic;
using Agricosmic.UI;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private SimpleButton storeButton;
    [SerializeField] private SimpleButton fishLibButton;
    [SerializeField] private SpriteRenderer moneyBackground;
    [SerializeField] private TextMeshPro moneyText;
    
    private MoveBetween _storeButtonMoveBetween;
    private MoveBetween _fishLibButtonMoveBetween;
    private MoveBetween _moneyBGMoveBetween;
    
    //[Header("Store")]
    
    //[Header("Fish Library")]

    private void Start()
    {
        SeymourController.onStartFishing += DisableHUD;
        SeymourController.onStopFishing += EnableHUD;
        
        _storeButtonMoveBetween = storeButton.GetComponent<MoveBetween>();
        _fishLibButtonMoveBetween = fishLibButton.GetComponent<MoveBetween>();;
        _moneyBGMoveBetween = moneyBackground.GetComponent<MoveBetween>();;
    }

    private void Update()
    {
        // TODO: Change to update to money
        moneyText.text = "Money: " + GameManager.I.CurrentMoney;
    }

    public void ShowStore()
    {
        Debug.Log("Store shown");
    }
    
    public void ShowFishLibrary()
    {
        Debug.Log("Library shown");
    }
    
    public void HideStore()
    {
        
    }
    
    public void HideFishLibrary()
    {
        
    }
    
    public void EnableHUD()
    {
        storeButton.Active = true;
        fishLibButton.Active = true;
        
        _storeButtonMoveBetween.MoveIn();
        _fishLibButtonMoveBetween.MoveIn();
        _moneyBGMoveBetween.MoveIn();
    }

    public void DisableHUD()
    {
        storeButton.Active = false;
        fishLibButton.Active = false;
        
        _storeButtonMoveBetween.MoveOut();
        _fishLibButtonMoveBetween.MoveOut();
        _moneyBGMoveBetween.MoveOut();
    }
}
