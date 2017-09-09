using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    CanvasGroup canvasGroup;

    [NonSerialized]
    public Transform startParent;
    [NonSerialized]
    public Vector3 startPosition;
    [NonSerialized]
    public ItemDropHandler startSlot;
    
    //public Item itemScript;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startParent = transform.parent;
        startPosition = transform.position;
        startSlot = transform.parent.GetComponent<ItemDropHandler>();
        canvasGroup.blocksRaycasts = false;

        transform.parent = Inventory.instance.topOfCanvas;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if(transform.parent == Inventory.instance.topOfCanvas)
        {
            transform.position = startPosition;
            transform.parent = startParent;
        }
    }
}