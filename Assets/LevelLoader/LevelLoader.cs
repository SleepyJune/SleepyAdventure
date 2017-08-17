using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class LevelLoader : MonoBehaviour {

    public PrefabManager prefabManager;
    public GameObject playerPrefab;

    GameObject levelHolder;

    Level level;

    void Start () {
        levelHolder = new GameObject("LevelHolder");
        level = new Level();

        Load();
    }

    void CreateNewObject(int cid, int id, IPosition pos)
    {
        var selectedOriginal = prefabManager.collections[cid].objects[id];

        if(selectedOriginal.tag == "Start")
        {
            selectedOriginal = playerPrefab;
        }

        var newObject = Instantiate(selectedOriginal, new Vector3(pos.x, pos.y, pos.z), new Quaternion(), levelHolder.transform);
        
    }

    public void Load()
    {
        string path = Application.dataPath + "/Saves/";

        if (File.Exists(path + level.name + ".json"))
        {            
            string str = File.ReadAllText(path + level.name + ".json");

            var sqrObjects = JsonHelper.FromJson<SquareObject>(str);

            foreach (var obj in sqrObjects)
            {
                Square square = new Square(obj.pos);
                square.objects.Add(obj);

                var selectedOriginal = prefabManager.collections[obj.cid].objects[obj.id];
                if (level.AddSquareObject(obj.pos, obj.cid, obj.id, selectedOriginal) != null)
                {
                    CreateNewObject(obj.cid, obj.id, obj.pos);
                }

                //level.map.Add(square.position, square);
            }


            //level.LoadLevel(str);
        }
    }

}
