using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FishingPopup : MonoBehaviour
{
    [SerializeField] private MoveBetween moveBetween;
    [SerializeField] private Animator animator;

    public void Appear()
    {
        moveBetween.MoveIn();
        animator.SetTrigger("Appear");
    }
}
