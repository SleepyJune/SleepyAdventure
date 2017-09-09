using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public PrefabCollection[] collections;

    public PrefabCollection itemCollection;
    public Dictionary<int, Item> itemDatabase;

    public static PrefabManager instance = null;

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

        InitializeItems();
    }

    void InitializeItems()
    {
        if (itemCollection)
        {
            itemDatabase = new Dictionary<int, Item>();

            foreach (var obj in itemCollection.objects)
            {
                if (obj)
                {
                    var item = obj.GetComponent<Item>();
                    if (item)
                    {
                        itemDatabase.Add(item.itemId, item);
                    }
                }
            }
        }
    }

    public Item GetItemPrefab(int itemID)
    {
        Item item;
        if(itemDatabase.TryGetValue(itemID, out item))
        {
            return item;
        }

        return null;
    }
}
