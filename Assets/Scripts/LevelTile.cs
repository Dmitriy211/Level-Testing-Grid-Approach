using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class LevelTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Color _mainColor;
    [SerializeField] private Color _alternativeColor;
    [SerializeField] private Color _highlightColor;

    private bool _isMain;
    private SpriteRenderer _spriteRenderer;
    private LevelEditor _levelEditor;
    
    public int Position { get; set; }

    public bool IsMain
    {
        get => _isMain;
        set
        {
            _isMain = value;
            ResetColor();
        }
    }
    

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _levelEditor = GetComponentInParent<LevelEditor>();
    }

    public void Highlight()
    {
        _spriteRenderer.color = _highlightColor;
    }

    public void ResetColor()
    {
        _spriteRenderer.color = IsMain ? _mainColor : _alternativeColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Highlight();
        
        _levelEditor.OnTileHover(Position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetColor();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _levelEditor.OnTileClick(Position);
        _levelEditor.MouseHeld = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _levelEditor.MouseHeld = false;
    }
}
