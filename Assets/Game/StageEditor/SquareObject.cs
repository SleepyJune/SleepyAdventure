using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SquareObject
{
    public IPosition pos;
    public Vector3 rotation;
    public int pid;
        
    private GameObject obj;

    public SquareObject(IPosition pos, GameObject obj)
    {
        this.obj = obj;
        this.pos = pos;

        this.pid = obj.GetComponent<PrefabObject>().prefabID;
        
        if(pid <= 0)
        {
            throw new System.Exception("PrefabID of 0 on prefab " + obj.name);
        } 
    }

    public SquareObject(IPosition pos, Vector3 rotation, GameObject obj)
    {
        this.obj = obj;
        this.pos = pos;
        this.rotation = rotation;

        this.pid = obj.GetComponent<PrefabObject>().prefabID;

        if (pid <= 0)
        {
            throw new System.Exception("PrefabID of 0 on prefab " + obj.name);
        }
    }

    public GameObject GetGameObject()
    {
        return obj;
    }

    public void SetGameObject(GameObject obj)
    {
        this.obj = obj;
    }
}
