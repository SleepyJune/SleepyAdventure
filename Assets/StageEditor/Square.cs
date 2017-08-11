using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square {

    public Vector2 position;
    public List<SquareObject> objects;

    public Square(Vector2 pos)
    {
        this.position = pos;
        this.objects = new List<SquareObject>();
    }
}
