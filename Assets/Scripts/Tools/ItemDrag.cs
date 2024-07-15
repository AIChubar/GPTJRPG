using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// Script that makes the GameObject it assigned draggable.
/// </summary>
public class ItemDrag : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler
{
    public RectTransform rectTransform;
    /// <summary>
    /// Item position before it started being dragged.
    /// </summary>
    [HideInInspector] public Vector2 startPosition;
    
    /// <summary>
    /// Is item available to be dragged.
    /// </summary>
    [HideInInspector] public bool active = true;

    [SerializeField] private SwappingInterface swapper;
    
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
        swapper = GetComponentInParent<SwappingInterface>();
    }

    /// <summary>
    /// Returns item to the initial position.
    /// </summary>
    public void ResetPosition()
    {
        rectTransform.anchoredPosition = startPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (active && !GameManager.gameManager.inBattle)
            swapper.SwapCurrentItem();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (active && !GameManager.gameManager.inBattle)
            swapper.SetCurrentItem(rectTransform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (active && !GameManager.gameManager.inBattle)
            rectTransform.anchoredPosition += eventData.delta / swapper.canvas.scaleFactor;
    }
}