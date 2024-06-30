using System;
using UnityEngine;

public class StoreInfo : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private SpriteRenderer itemIcon;
    [SerializeField] private int itemCost;
    
    // Choose one
    [SerializeField] private LureType lureType;
    [SerializeField] private HookType hookType;
    
    [SerializeField] private Color _color = Color.white;
    [SerializeField] private bool _enabledOnStart = true;
    [SerializeField] private float _preferredWidth = 55;
    private bool _disable;
        
    private void Start()
    {
        if (!_enabledOnStart) enabled = false;
    }

    public string GetName() => itemName;
    public void SetName(string str) => itemName = str;
    public int GetCost() => itemCost;
    public Sprite GetIcon() => itemIcon.sprite;
    public LureType GetLureUnlock() => lureType;
    public HookType GetHookUnlock() => hookType;
    public Color GetColor() => _color;
    public void SetColor(Color tipColor) => _color = tipColor;
    public float GetPreferredWidth() => _preferredWidth;
    public void SetPreferredWidth(float preferredWidth) => _preferredWidth = preferredWidth;
    public void Disable() => _disable = true;
    public void Enable() => _disable = false;
        
    private void OnMouseEnter()
    {
        if (enabled && !_disable)
            InfoPanelController.I.TooltipEntered(this);
    }

    private void OnMouseExit()
    {
        if (enabled && !_disable)
            InfoPanelController.I.TooltipExited(this);
    }

    private void OnDisable()
    {
        if (InfoPanelController.I != null)
            if (InfoPanelController.I.GetCurrentTooltip() == this)    
                InfoPanelController.I.TooltipExited(this);
    }
}