using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [System.NonSerialized]
    public int id;
    [System.NonSerialized]
    public Square sqr;
    [System.NonSerialized]
    public IPosition pos2d;
        
    public Vector3 pos
    {
        get
        {
            return transform.position;
        }
    }

    [System.NonSerialized]
    public bool isDead = false;

    public bool Equals(Entity obj)
    {
        return obj.id == this.id;
    }

    public override int GetHashCode()
    {
        return this.id.GetHashCode();
    }
}