using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrag : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler
{
    public RectTransform rectTransform;
    public Vector2 startPosition;
    
    [HideInInspector] public bool active = true;

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