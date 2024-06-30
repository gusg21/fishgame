using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InfoPanelController : MonoBehaviour
    {
        public static InfoPanelController I;
        
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private GameObject _tooltipUI;
        [SerializeField] private float _edgeRatio = 0.8f;

        private StoreInfo _tip;

        private void Start()
        {
            I = this;
        }

        private void OnDestroy()
        {
            I = null;
        }

        public void TooltipEntered(StoreInfo tip)
        {
            _tip = tip;
            _tooltipUI.GetComponentInChildren<LayoutElement>().preferredWidth = _tip.GetPreferredWidth();
        }

        public void TooltipExited(StoreInfo tip)
        {
            _tip = null;
        }

        public StoreInfo GetCurrentTooltip() => _tip;

        private void Update()
        {
            _tooltipUI.SetActive(_tip != null);

            if (_tip != null)
            {
                _nameText.text = String.IsNullOrWhiteSpace(_tip.GetName()) ? "ERR" : _tip.GetName();
                _costText.text = _tip.GetCost().ToString();
            }

            // Determine flip flags
            var worldPos = transform.position;
            var mouseNearRight = 
                Camera.main.WorldToScreenPoint(worldPos).x > Screen.width * _edgeRatio;
            var mouseNearBottom = 
                Camera.main.WorldToScreenPoint(worldPos).y < Screen.width * (1f - _edgeRatio);
        }
    }