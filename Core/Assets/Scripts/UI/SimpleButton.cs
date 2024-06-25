using UnityEngine;
using UnityEngine.Events;

namespace Agricosmic.UI
{
    /// <summary>
    /// Represents something clickable and hoverable
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class SimpleButton : MonoBehaviour
    {
        [Header("Sprites")]
        [Tooltip("The sprite to use when not hovered")]
        [SerializeField] private Sprite _normalSprite;
        [Tooltip("The sprite to use when hovered")]
        [SerializeField] private Sprite _hoverSprite;

        [Header("Button Data")] [Tooltip("Called when the button is clicked. Passes this button")]
        [SerializeField] private UnityEvent<SimpleButton> _onClick;
        [Header("References")]
        [Tooltip("The sprite renderer to update the sprite of")]
        [SerializeField] private SpriteRenderer _renderer;
        
        private bool _isMouseOver = false;

        private void Start()
        {
            FindReferences();
        }

        protected void FindReferences()
        {
            if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();

            var box = GetComponent<BoxCollider2D>();
            box.size = new(_renderer.size.x, box.size.y);
        }

        protected SpriteRenderer GetRenderer() => _renderer;
        
        private void Update()
        {
            _renderer.sprite = _isMouseOver ? _hoverSprite : _normalSprite;
        }

       
        private void OnMouseEnter()
        {
            if (GameManager.CurrentMouseState == MouseState.DEFAULT)
                _isMouseOver = true;
        }

        private void OnMouseExit()
        {
            _isMouseOver = false;
        }

        private void OnMouseDown()
        {
            _isMouseOver = false;
            _onClick?.Invoke(this); 
        }

        private void OnMouseUp()
        {
            if (GameManager.CurrentMouseState == MouseState.DEFAULT)
                _isMouseOver = true;
        }
    }
}