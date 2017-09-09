using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public Sprite image;

    public int itemId;

    public string itemName;
    public string description;

    public abstract bool Use();//Unit source);
}
