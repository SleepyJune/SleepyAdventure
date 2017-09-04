using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PrefabCollection : ScriptableObject {    
    public float height;    
    
    public Sprite[] images = new Sprite[0];
    public GameObject[] objects = new GameObject[0];
}
