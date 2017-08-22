using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level{

    public string name = "New Level";
    public Dictionary<IPosition, Square> map;   
    
    public Level()
    {
        map = new Dictionary<IPosition, Square>();
    }

    public string SaveLevel()
    {
        List<SquareObject> objects = new List<SquareObject>();

        foreach(var square in map.Values)
        {
            if (square != null)
            {
                foreach (var obj in square.objects)
                {
                    objects.Add(obj);
                }
            }
        }

        return JsonHelper.ToJson<SquareObject>(objects.ToArray());
    }

    public void RemoveSquareObject(IPosition pos)
    {
        map.Remove(pos);
    }

    public SquareObject AddSquareObject(IPosition pos, int cid, int id, GameObject obj)
    {
        return AddSquareObject(pos, Vector3.zero, cid, id, obj);
    }

    public SquareObject AddSquareObject(IPosition pos, Vector3 rotation, int cid, int id, GameObject obj)
    {
        var square = this.GetSquareAtPoint(pos.To2D());

        if (obj.tag == "Start")
        {
            foreach(var tsquare in map.Values)
            {
                if (tsquare.objects.Any(o => o.GetGameObject().tag == "Start"))
                {
                    Debug.Log("Duplicate Starts");
                    return null; // can't have duplicate starts
                }
            }            
        }

        if (square == null)
        {
            SquareObject newObj = new SquareObject(pos, rotation, cid, id, obj);
            square = new Square(pos.To2D());
            square.objects.Add(newObj);

            map.Add(square.position, square);

            return newObj;
        }
        else
        {            
            if(square.objects.Any(o => o.pos == pos))
            {
                return null;
            }
            else
            {
                SquareObject newObj = new SquareObject(pos, rotation, cid, id, obj);
                square.objects.Add(newObj);
                
                return newObj;
            }
        }

        return null;
    }

    public Square GetSquareAtPoint(IPosition pos)
    {
        Square square;
        if(map.TryGetValue(pos, out square))
        {
            return square;
        }

        return null;
    }
    	
}
