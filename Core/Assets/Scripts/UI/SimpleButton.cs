using UnityEngine;
using UnityEngine.Events;

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
        
    public bool Active { get; set; }
        
    private bool _isMouseOver = false;

    private void Start()
    {
        FindReferences();
        Active = true;
    }

    protected void FindReferences()
    {
        if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();
    }

    protected SpriteRenderer GetRenderer() => _renderer;
        
    private void Update()
    {
        _renderer.sprite = _isMouseOver ? _hoverSprite : _normalSprite;
    }

    public SpriteRenderer GetSpriteRenderer() => _renderer;
       
    private void OnMouseEnter()
    {
        if (GameManager.CurrentMouseState == MouseState.DEFAULT && Active)
        {
            _isMouseOver = true;
        }
    }

    private void OnMouseExit()
    {
        _isMouseOver = false;
    }

    private void OnMouseDown()
    {
        if (_isMouseOver)
            _onClick?.Invoke(this); 
            
        _isMouseOver = false;
    }

    private void OnMouseUp()
    {
        if (GameManager.CurrentMouseState == MouseState.DEFAULT && Active)
        {
            _isMouseOver = true;
        }
    }
}