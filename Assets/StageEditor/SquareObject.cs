using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SquareObject
{
    public Vector2 pos;
    public int cid;
    public int id;
    GameObject obj;

    public SquareObject(Vector2 pos, int cid, int id, GameObject obj)
    {
        this.obj = obj;
        this.cid = cid;
        this.id = id;
        this.pos = pos;
    }

    public GameObject GetGameObject()
    {
        return obj;
    }
}
