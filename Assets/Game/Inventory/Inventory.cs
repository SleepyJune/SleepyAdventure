using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using System.IO;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public GameObject slotPrefab;
    public GameObject itemPrefab;


    [System.NonSerialized]
    public Equipment equipment;

    public Transform InfoPanel;

    public Transform topOfCanvas;

    public int maxSlots = 25;
    public int[] slots;

    string savePath;

    const string saveFilename = "Inventory.save";

    Transform list;

    CanvasGroup canvasGroup;

    bool inventoryKeyPress = false;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
                
        if (Application.platform == RuntimePlatform.Android)
        {
            savePath = Application.persistentDataPath + "/Saves/";
        }
        else
        {
            savePath = Application.dataPath + "/Saves/";
        }

        equipment = GetComponent<Equipment>();

        list = transform.Find("Panel/List");
        canvasGroup = GetComponent<CanvasGroup>();

        LoadInventory();
        InitializeSlots();
    }

    public void InitializeSlots()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            var slot = Instantiate(slotPrefab, list);
            var slotScript = slot.GetComponent<ItemDropHandler>();
            slotScript.itemSlotNumber = i;
            slotScript.inventory = this;

            var itemID = slots[i];

            if(itemID != 0)
            {
                var prefab = PrefabManager.instance.GetItemPrefab(itemID);

                if (prefab != null)
                {
                    var itemSlot = Instantiate(itemPrefab, slot.transform);

                    var item = Instantiate(prefab, itemSlot.transform);

                    var itemDragScript = itemSlot.GetComponent<ItemDragHandler>();
                    var itemScript = item.GetComponent<Item>();

                    itemDragScript.Initialize(itemScript);
                }
            }
        }
    }

    public void OpenInventory()
    {
        //inventoryPanel.SetActive(!inventoryPanel.activeSelf);

        if(canvasGroup.alpha == 0)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }
        
    }

    public void Save()
    {
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        /*var itemSlots = list.GetComponentsInChildren<ItemDropHandler>();

        foreach(var slot in itemSlots)
        {
            if (slot.item)
            {
                var item = slot.item.GetComponent<Item>();
                if (item)
                {
                    slots[slot.itemSlotNumber] = item.itemId;
                }
            }
        }*/

        var str = JsonHelper.ToJson<int>(this.slots);
        File.WriteAllText(savePath + saveFilename, str);
    }

    public void LoadInventory()
    {
        if (!Directory.Exists(savePath) || !File.Exists(savePath + saveFilename))
        {
            slots = new int[25];
            return;
        }

        string str = File.ReadAllText(savePath + saveFilename);
        slots = JsonHelper.FromJson<int>(str);
    }
}
