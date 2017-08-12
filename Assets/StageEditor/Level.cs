using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level{

    public string name = "New Level";
    public Dictionary<IPosition, Square> map;   
    
    public Level(int width, int height)
    {
        map = new Dictionary<IPosition, Square>();
    }
     
    public bool LoadLevel(string str)
    {
        var sqrObjects = JsonHelper.FromJson<SquareObject>(str);

        foreach(var obj in sqrObjects)
        {                        
            Square square = new Square(obj.pos);
            square.objects.Add(obj);

            //var newObject = manager.collections[obj.cid].objects[obj.id];

            if (!map.ContainsKey(square.position))
            {
                map.Add(square.position, square);
            }           
        }

        return false;
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
        
    public SquareObject AddSquareObject(Vector3 vPos, int cid, int id, GameObject obj)
    {
        var pos = vPos.ConvertToIPosition();
        var square = this.GetSquareAtPoint(pos);
        
        if(square == null)
        {
            SquareObject newObj = new SquareObject(pos, cid, id, obj);
            square = new Square(pos);
            square.objects.Add(newObj);

            map.Add(square.position, square);

            return newObj;
        }
        else
        {

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
