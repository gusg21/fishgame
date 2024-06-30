using System;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private string _tooltipString;
    [SerializeField] private Color _color = Color.white;
    [SerializeField] private bool _enabledOnStart = true;
    [SerializeField] private float _preferredWidth = 55;
    private bool _disable;
        
    private void Start()
    {
        if (!_enabledOnStart) enabled = false;
    }

    public string GetString() => _tooltipString;
    public void SetString(string str) => _tooltipString = str;
    public Color GetColor() => _color;
    public void SetColor(Color tipColor) => _color = tipColor;
    public float GetPreferredWidth() => _preferredWidth;
    public void SetPreferredWidth(float preferredWidth) => _preferredWidth = preferredWidth;
    public void Disable() => _disable = true;
    public void Enable() => _disable = false;
        
    private void OnMouseEnter()
    {
        if (enabled && !_disable)
            TooltipController.I.TooltipEntered(this);
    }

    private void OnMouseExit()
    {
        if (enabled && !_disable)
            TooltipController.I.TooltipExited(this);
    }

    private void OnDisable()
    {
        if (TooltipController.I != null)
            if (TooltipController.I.GetCurrentTooltip() == this)    
                TooltipController.I.TooltipExited(this);
    }
}