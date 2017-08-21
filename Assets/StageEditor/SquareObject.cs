using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SquareObject
{
    public IPosition pos;
    public int cid;
    public int id;
    GameObject obj;

    public SquareObject(IPosition pos, int cid, int id, GameObject obj)
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
