using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SquareObject
{
    public IPosition pos;
    public Vector3 rotation;
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

    public SquareObject(IPosition pos, Vector3 rotation, int cid, int id, GameObject obj)
    {
        this.obj = obj;
        this.cid = cid;
        this.id = id;
        this.pos = pos;
        this.rotation = rotation;
    }

    public GameObject GetGameObject()
    {
        return obj;
    }
}
