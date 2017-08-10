using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level{

    public string name = "New Level";
    public Square[,] map;   
    
    public Level(int width, int height)
    {
        map = new Square[width, height];
    }

    public bool LoadLevel(string str)
    {
        var sqrObjects = JsonHelper.FromJson<SquareObject>(str);

        foreach(var obj in sqrObjects)
        {                        
            Square square = new Square(obj.pos);
            square.objects.Add(obj);

            //var newObject = manager.collections[obj.cid].objects[obj.id];
                       
            map[(int)obj.pos.x, (int)obj.pos.y] = square;
        }

        return false;
    }

    public string SaveLevel()
    {
        List<SquareObject> objects = new List<SquareObject>();

        foreach(var square in map)
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

    public bool AddSquareObject(Vector3 point, int cid, int id, GameObject obj)
    {
        var square = this.GetSquareAtPoint(point);
        Vector2 vec2 = new Vector2(Mathf.Round(point.x), Mathf.Round(point.z));
        
        if(square == null)
        {
            SquareObject newObj = new SquareObject(vec2, cid, id, obj);
            square = new Square(vec2);
            square.objects.Add(newObj);

            map[(int)vec2.x, (int)vec2.y] = square;

            return true;
        }
        else
        {

        }

        return false;
    }

    public Square GetSquareAtPoint(Vector3 point)
    {
        return map[(int)Mathf.Round(point.x), (int)Mathf.Round(point.z)];
    }
    	
}
