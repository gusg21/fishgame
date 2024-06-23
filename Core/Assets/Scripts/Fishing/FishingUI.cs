using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingUI : MonoBehaviour
{
    [SerializeField] private MoveBetween fishingBackground;

    private void Start()
    {
        SeymourController.StartFishing += ChangeDirection;
        SeymourController.StartFishing += fishingBackground.MoveIn;
        SeymourController.StopFishing += fishingBackground.MoveOut;
    }
    
    public void ChangeDirection()
    {
        switch (GameManager.I.GetSeymour().GetSeymourState())
        {
            case SeymourState.FISHINGRIGHT:
                fishingBackground._posOut.x = Mathf.Abs(fishingBackground._posOut.x);
                fishingBackground._posIn.x = Mathf.Abs(fishingBackground._posIn.x);
                break;
            case SeymourState.FISHINGLEFT:
                fishingBackground._posOut.x = -Mathf.Abs(fishingBackground._posOut.x);
                fishingBackground._posIn.x = -Mathf.Abs(fishingBackground._posIn.x);
                break;
        }
    }
}
