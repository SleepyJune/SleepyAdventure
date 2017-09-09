using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using System.IO;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public GameObject slotPrefab;
    public GameObject itemPrefab;

    public Transform topOfCanvas;

    public int maxSlots = 25;
    public int[] slots;

    string savePath;

    const string saveFilename = "Inventory.save";

    Transform list;

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

        list = transform.Find("Panel/List");

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

            var itemID = slots[i];

            if(itemID != 0)
            {
                var prefab = PrefabManager.instance.GetItemPrefab(itemID);

                if (prefab != null)
                {
                    var itemSlot = Instantiate(itemPrefab, slot.transform);

                    var item = Instantiate(prefab, itemSlot.transform);
                    var itemScript = item.GetComponent<Item>();

                    var itemDragScript = itemSlot.GetComponent<ItemDragHandler>();
                    //itemDragScript.itemScript = itemScript;

                    //GetComponent<Button>().onClick.AddListener(()=>Debug.Log(GameManager.instance.gameCounter));
                    itemDragScript.GetComponent<Button>().onClick.AddListener(() => itemScript.Use());

                    //itemSlot.GetComponent<Image>().sprite = itemScript.image;
                }
            }
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
