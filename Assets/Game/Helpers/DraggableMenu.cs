using System;
using UnityEngine;
using UnityEngine.EventSystems;

class DraggableMenu : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    CanvasGroup canvasGroup;

    Vector3 startPosition;
    Vector3 diff;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //canvasGroup.blocksRaycasts = false;

        startPosition = transform.position;
        diff = transform.position - Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = diff + Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //canvasGroup.blocksRaycasts = true;
    }
}
