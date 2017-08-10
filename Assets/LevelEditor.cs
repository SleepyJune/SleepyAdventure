using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class LevelEditor : MonoBehaviour
{

    public PrefabManager prefabManager;

    public GameObject defaultSquare;

    public int width = 20;
    public int height = 20;


    GameObject selectedObject;

    int selectedCollectionID = -1;
    int selectedID = -1;
    Level level;

    int shootableMask;


    // Use this for initialization
    void Start()
    {

        level = new Level(width, height);
        shootableMask = LayerMask.GetMask("Floor");

        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                var add = new Vector3(x, 0, y);
                Instantiate(defaultSquare, transform.position + add, transform.rotation);
            }
        }

        //prefabManager = GetComponent<PrefabManager>();

        for (int collectionID = 0;collectionID < prefabManager.collections.Length;collectionID++)
        {
            var collection = prefabManager.collections[collectionID];
            var firstObject = collection.objects[0];

            var newObject = Instantiate(firstObject, transform.position, transform.rotation);

            MonoBehaviour[] comps = newObject.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour c in comps)
            {
                c.enabled = false;
            }
            //newObject.GetComponent<Animator>().enabled = true;
            //newObject.GetComponent<Animator>().enabled = true;

            selectedObject = newObject;
            selectedCollectionID = collectionID;
            selectedID = 0;
        }

    }

    Vector3 GetRoundedPosition(Vector3 point)
    {
        return new Vector3(Mathf.Round(point.x), 0, Mathf.Round(point.z));
    }
    
    public void Load()
    {
        string path = Application.dataPath + "/Saves/";

        if(File.Exists(path + level.name + ".json"))
        {
            string str = File.ReadAllText(path + level.name + ".json");

            var sqrObjects = JsonHelper.FromJson<SquareObject>(str);

                Debug.Log(sqrObjects.Length);
            foreach (var obj in sqrObjects)
            {
                Square square = new Square(obj.pos);
                square.objects.Add(obj);

                var newObject = prefabManager.collections[obj.cid].objects[obj.id];

                Instantiate(newObject, new Vector3(obj.pos.x,0,obj.pos.y), new Quaternion());

                level.map[(int)obj.pos.x, (int)obj.pos.y] = square;
            }


            //level.LoadLevel(str);
        }
    }

    public void Save()
    {
        string str = level.SaveLevel();

        string path = Application.dataPath + "/Saves/";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        File.WriteAllText(path + level.name + ".json", str);
        Debug.Log(str);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, shootableMask))
            {
                Vector3 playerToMouse = hit.point - transform.position;
                playerToMouse.y = 0f;
                
                if (level.AddSquareObject(hit.point, selectedCollectionID, selectedID, selectedObject))
                {
                    Instantiate(selectedObject, GetRoundedPosition(hit.point), new Quaternion());
                }
                else
                {
                    
                }
            }
        }
    }
}
