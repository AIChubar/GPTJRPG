using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Class that is used for inheritance. Provide an interface for dragging and dropping <see cref="ItemDrag"/>.
/// </summary>
public class SwappingInterface : MonoBehaviour
{
    [SerializeField] protected List<ItemDrag> items;
    
    public Canvas canvas;

    private RectTransform currentDraggedItem;

    protected void Start()
    {
        PopulateItems();
    }

    private void PopulateItems()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var item = transform.GetChild(i).GetComponent<ItemDrag>();
            if (item is not null && ! item.Equals(null))
                items.Add(item);
        }
    }

    /// <summary>
    /// Sets the item that is currently being dragged.
    /// </summary>
    public void SetCurrentItem(RectTransform item)
    {
        currentDraggedItem = item;
    }

    private void ResetItem()
    {
        currentDraggedItem = null;
    }

    /// <summary>
    /// Method that attempts to swap currently dragged item with the one under the mouth cursor.
    /// </summary>
    public void SwapCurrentItem()
    {
        if (currentDraggedItem == null)
        {
            return;
        }

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            ItemDrag targetItem = result.gameObject.GetComponent<ItemDrag>();
            if (targetItem != null && targetItem.rectTransform != currentDraggedItem)
            {

                // Swap items in the list (if needed)
                int currentIndex = items.FindIndex(item => item.rectTransform == currentDraggedItem);
                int targetIndex = items.FindIndex(item => item.rectTransform == targetItem.rectTransform);
                
                
                Swap(currentIndex, targetIndex);
                break; 
            }
        }

        currentDraggedItem.GetComponent<ItemDrag>().ResetPosition();

        ResetItem();
    }

    protected virtual void Swap(int currentIndex, int targetIndex)
    {
        
    }
}
