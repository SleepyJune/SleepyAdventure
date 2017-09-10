using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    CanvasGroup canvasGroup;

    [NonSerialized]
    public Transform startParent;
    [NonSerialized]
    public Vector3 startPosition;
    [NonSerialized]
    public ItemDropHandler startSlot;
    [NonSerialized]
    public Image background;
    
    public Item itemScript;

    public Color equipedBackground;

    [System.NonSerialized]
    public Color normalBackground;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        background = GetComponent<Image>();

        normalBackground = GetComponent<Image>().color;
    }

    public void Initialize(Item item)
    {
        itemScript = item;


        //itemDragScript.itemScript = itemScript;
        
        GetComponent<Button>().onClick.AddListener(() => UseItem());
        //GetComponent<EventTrigger>.

        //itemSlot.GetComponent<Image>().sprite = itemScript.image;

    }

    public void UseItem()
    {
        if (itemScript.Use())
        {
            if(itemScript is WeaponItem)
            {
                background.color = equipedBackground;
            }
        }
        else
        {
            background.color = normalBackground;
        }
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        Inventory.instance.InfoPanel.GetComponent<CanvasGroup>().alpha = 1f;
        //Inventory.instance.InfoPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

        Inventory.instance.InfoPanel.Find("Panel/Name").GetComponent<Text>().text = itemScript.itemName;
        Inventory.instance.InfoPanel.Find("Panel/Description").GetComponent<Text>().text = itemScript.description;

        Inventory.instance.InfoPanel.transform.position = transform.position;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Inventory.instance.InfoPanel.GetComponent<CanvasGroup>().alpha = 0f;
        //Inventory.instance.InfoPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}