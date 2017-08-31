using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [System.NonSerialized]
    public int id;
    public Square sqr;
    public IPosition pos2d;

    public bool Equals(Entity obj)
    {
        return obj.id == this.id;
    }

    public override int GetHashCode()
    {
        return this.id.GetHashCode();
    }
}