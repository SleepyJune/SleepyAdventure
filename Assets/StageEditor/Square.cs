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
}
