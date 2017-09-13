using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square {

    public IPosition position;
    public List<SquareObject> objects;
    public Dictionary<int, Entity> obstacles;

    public bool isWalkable;

    //public int walkableCounter = 0;

    public Square(IPosition pos)
    {
        this.position = pos;
        this.objects = new List<SquareObject>();

        this.obstacles = new Dictionary<int, Entity>();

        this.isWalkable = true;        
    }

    public void CheckWalkable()
    {
        foreach (var sqrObj in objects)
        {
            var gameObj = sqrObj.GetGameObject();

            if (gameObj.tag == "Hazard")
            {
                isWalkable = false;
            }
            else if (gameObj.tag == "Wall")
            {
                isWalkable = false;
            }
        }
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
