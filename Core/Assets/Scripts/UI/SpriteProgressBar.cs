using UnityEngine;

namespace Agricosmic.Utilities
{
    /// <summary>
    /// Updates a sprite's width based on a percentage
    /// </summary>
    [ExecuteInEditMode]
    public class SpriteProgressBar : MonoBehaviour
    {
        [Tooltip("The width to use when value = 0")] [SerializeField]
        private float _minWidth = 1.11f;

        [Tooltip("The width to use when value = 1")] [SerializeField]
        private float _maxWidth = 8f;

        [Tooltip("The bar renderer")] [SerializeField]
        private SpriteRenderer _renderer;

        [Tooltip("The background renderer")] [SerializeField]
        private SpriteRenderer _backgroundRenderer;

        [Tooltip("The normalized progress to show")] [SerializeField]
        [Range(0, 1)]
        private float _value;

        /// <summary>
        /// Hide the progress bar
        /// </summary>
        public void Hide()
        {
            _renderer.color = Color.clear;
            if (_backgroundRenderer != null)
                _backgroundRenderer.color = Color.clear;
        }

        /// <summary>
        /// Show the progress bar
        /// </summary>
        public void Show()
        {
            _renderer.color = Color.white;
            if (_backgroundRenderer != null)
                _backgroundRenderer.color = Color.white;
        }

        /// <summary>
        /// Set the progress
        /// </summary>
        /// <param name="value">NORMALIZED value</param>
        public void SetValue(float value)
        {
            _value = Mathf.Clamp01(value);
            var rendererSize = _renderer.size;
            rendererSize.x = Mathf.Lerp(_minWidth, _maxWidth, _value);
            _renderer.size = rendererSize;
        }

#if UNITY_EDITOR
        public void Update()
        {
            SetValue(_value);
        }
#endif
    }
}