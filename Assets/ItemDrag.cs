using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrag : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler
{
    public RectTransform rectTransform;
    public Vector2 startPosition;

    [SerializeField] private SwappingInterface swapper;
    
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
        swapper = GetComponentInParent<SwappingInterface>();
    }

    public void ResetPosition()
    {
        rectTransform.anchoredPosition = startPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        swapper.SwapCurrentItem();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        swapper.SetCurrentItem(rectTransform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        rectTransform.anchoredPosition += eventData.delta / swapper.canvas.scaleFactor;
    }
}