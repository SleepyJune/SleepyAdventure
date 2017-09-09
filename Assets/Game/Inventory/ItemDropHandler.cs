using System;
using UnityEngine;
using UnityEngine.EventSystems;

class ItemDropHandler : MonoBehaviour, IDropHandler
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
                    Inventory.instance.slots[itemSlotNumber] = itemScript.itemId;
                    Inventory.instance.slots[draggedItem.startSlot.itemSlotNumber] = 0;

                    draggedItem.transform.SetParent(transform);
                }
                else
                {
                    var currentItemScript = item.gameObject.GetComponentInChildren<Item>();

                    Inventory.instance.slots[itemSlotNumber] = itemScript.itemId;
                    Inventory.instance.slots[draggedItem.startSlot.itemSlotNumber] = currentItemScript.itemId;

                    draggedItem.transform.SetParent(transform);
                    item.transform.SetParent(draggedItem.startParent);
                }

                Inventory.instance.Save();
            }
        }
    }
}