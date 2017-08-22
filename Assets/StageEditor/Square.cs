using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square {

    public IPosition position;
    public List<SquareObject> objects;

    public Square(IPosition pos)
    {
        this.position = pos;
        this.objects = new List<SquareObject>();
    }

    public override bool Equals(object obj)
    {
        if (obj is Square)
        {
            return Equals((Square)this);
        }

        return false;
    }

    public bool Equals(Square obj)
    {
        return obj.position == this.position;
    }

    public override int GetHashCode()
    {
        return this.position.GetHashCode();
    }
}
