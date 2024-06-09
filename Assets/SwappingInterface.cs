using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public void SetCurrentItem(RectTransform item)
    {
        currentDraggedItem = item;
    }

    public void ResetItem()
    {
        currentDraggedItem = null;
    }

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

        bool foundValidDropTarget = false;
        foreach (RaycastResult result in results)
        {
            ItemDrag targetItem = result.gameObject.GetComponent<ItemDrag>();
            if (targetItem != null && targetItem.rectTransform != currentDraggedItem)
            {

                // Swap items in the list (if needed)
                int currentIndex = items.FindIndex(item => item.rectTransform == currentDraggedItem);
                int targetIndex = items.FindIndex(item => item.rectTransform == targetItem.rectTransform);
                
                
                Swap(currentIndex, targetIndex);
                /*if (currentIndex != -1 && targetIndex != -1)
                {
                    ItemDrag tempItem = items[currentIndex];
                    items[currentIndex] = items[targetIndex];
                    items[targetIndex] = tempItem;
                }*/
                /*// Swap positions
                Vector2 tempStartPosition = targetItem.startPosition;
                targetItem.startPosition = currentDraggedItem.GetComponent<ItemDrag>().startPosition;
                currentDraggedItem.GetComponent<ItemDrag>().startPosition = tempStartPosition;
                // Update transform positions
                currentDraggedItem.position = currentDraggedItem.GetComponent<ItemDrag>().startPosition;
                targetItem.rectTransform.position = targetItem.startPosition;*/
                

                foundValidDropTarget = true;
                break; // Exit the loop after the first valid target item
            }
        }

        //if (!foundValidDropTarget)
        //{
            // Reset the dragged item to its initial position
            currentDraggedItem.GetComponent<ItemDrag>().ResetPosition();
        //}

        ResetItem();
    }

    protected virtual void Swap(int currentIndex, int targetIndex)
    {
        
    }
}
