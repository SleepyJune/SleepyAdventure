using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorDisplayObject : MonoBehaviour {

    public int cid;
    public int id;

    public Vector3 pos;

    SquareObject sqrObject;

    public void RemoveObject()
    {
        Destroy(gameObject);
        sqrObject = null;
    }
}
