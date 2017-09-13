using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public PrefabCollection[] collections;

    public PrefabCollection itemCollection;
    public Dictionary<int, Item> itemDatabase;
    public Dictionary<int, GameObject> prefabDatabase;

    public static PrefabManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        InitializePrefabs();
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
        if (itemDatabase.TryGetValue(itemID, out item))
        {
            return item;
        }

        return null;
    }

    void InitializePrefabs()
    {
        prefabDatabase = new Dictionary<int, GameObject>();

        foreach (var collection in collections)
        {
            if (collection)
            {
                foreach (var obj in collection.objects)
                {
                    if (obj)
                    {
                        var prefabInfo = obj.GetComponent<PrefabObject>();

                        if (prefabInfo)
                        {
                            if (prefabInfo.prefabID > 0)
                            {
                                prefabDatabase.Add(prefabInfo.prefabID, obj);
                            }
                        }
                    }
                }
            }
        }
    }

    public GameObject GetGameObject(int prefabID)
    {
        GameObject prefab;
        if (prefabDatabase.TryGetValue(prefabID, out prefab))
        {
            return prefab;
        }

        return null;
    }
}
