using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorDisplayObject : MonoBehaviour {

    public PrefabCollection collection;

    public int pid;

    public IPosition pos;

    SquareObject sqrObject;

    public void RemoveObject()
    {
        Destroy(gameObject);
        sqrObject = null;
    }
}
