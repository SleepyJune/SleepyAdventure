using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    public GameObject item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }

            return null;
        }
    }

    public int itemSlotNumber;

    [NonSerialized]
    public Inventory inventory;

    public void OnDrop(PointerEventData data)
    {
        if (data.pointerDrag != null)
        {
            var draggedItem = data.pointerDrag.GetComponent<ItemDragHandler>();

            if (draggedItem != null)
            {
                var itemScript = draggedItem.gameObject.GetComponentInChildren<Item>();

                if (item == null)
                {
                    inventory.slots[draggedItem.startSlot.itemSlotNumber] = 0;
                    inventory.slots[itemSlotNumber] = itemScript.itemId;

                    draggedItem.transform.SetParent(transform);
                }
                else
                {
                    var currentItemScript = item.gameObject.GetComponentInChildren<Item>();

                    inventory.slots[itemSlotNumber] = itemScript.itemId;
                    inventory.slots[draggedItem.startSlot.itemSlotNumber] = currentItemScript.itemId;

                    draggedItem.transform.SetParent(transform);
                    item.transform.SetParent(draggedItem.startParent);
                }

                inventory.Save();
            }
        }
    }
}