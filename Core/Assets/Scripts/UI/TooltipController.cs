using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TooltipController : MonoBehaviour
    {
        public static TooltipController I;
        
        [SerializeField] private TMP_Text _text;
        [SerializeField] private GameObject _tooltipUI;
        [SerializeField] private float _edgeRatio = 0.8f;
        [SerializeField] private float _verticalOffsetOnFlip = 40f;

        private Tooltip _tip;

        private void Start()
        {
            I = this;
        }

        private void OnDestroy()
        {
            I = null;
        }

        public void TooltipEntered(Tooltip tip)
        {
            _tip = tip;
            _tooltipUI.GetComponentInChildren<LayoutElement>().preferredWidth = _tip.GetPreferredWidth();
        }

        public void TooltipExited(Tooltip tip)
        {
            _tip = null;
        }

        public Tooltip GetCurrentTooltip() => _tip;

        private void Update()
        {
            _tooltipUI.SetActive(_tip != null);

            if (_tip != null)
            {
                var str = _tip.GetString();
                _text.text = String.IsNullOrWhiteSpace(str) ? "ERR" : str;
            }

            // Determine flip flags
            var worldPos = transform.position;
            var mouseNearRight = 
                Camera.main.WorldToScreenPoint(worldPos).x > Screen.width * _edgeRatio;
            var mouseNearBottom = 
                Camera.main.WorldToScreenPoint(worldPos).y < Screen.width * (1f - _edgeRatio);

            // Align text right if needed
            _text.alignment = mouseNearRight ? TextAlignmentOptions.Right : TextAlignmentOptions.Left;
        }
    }