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
    GameObject selectedOriginal;

    EditorDisplayObject selectedInfo;

    Level level;

    GameObject templateLevelHolder;
    GameObject levelHolder;
    GameObject menuObjectHolder;

    public GameObject menuObjectSpawnPoint;

    int menuItems = 0;

    int shootableMask;
    int editorMenuObjectMask;
    int editorObjectMask;

    bool clicked = false;
    
    public GameObject EraseButton;

    GameObject stageSelectedObject;

    // Use this for initialization
    void Start()
    {

        level = new Level(width, height);
        levelHolder = new GameObject("LevelHolder");
        templateLevelHolder = new GameObject("TemplateLevelHolder");

        shootableMask = LayerMask.GetMask("Floor");
        editorObjectMask = LayerMask.GetMask("EditorObject");
        editorMenuObjectMask = LayerMask.GetMask("EditorMenuObject");

        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                var add = new Vector3(x, 0, y);
                Instantiate(defaultSquare, transform.position + add, transform.rotation, templateLevelHolder.transform);
            }
        }

        //prefabManager = GetComponent<PrefabManager>();

        menuObjectHolder = new GameObject("MenuObjectHolder");

        menuObjectHolder.transform.parent = menuObjectSpawnPoint.transform;
        menuObjectHolder.transform.localPosition = Vector3.zero;
        menuObjectHolder.transform.localRotation = Quaternion.identity;

        for (int collectionID = 0; collectionID < prefabManager.collections.Length; collectionID++)
        {
            var collection = prefabManager.collections[collectionID];

            for (int objectID = 0; objectID < collection.objects.Length; objectID++)
            {
                var original = collection.objects[objectID];

                var newObject = Instantiate(original, menuObjectHolder.transform);
                newObject.transform.localPosition = Vector3.zero;
                newObject.transform.localRotation = Quaternion.identity;

                newObject.transform.localPosition += new Vector3(-menuItems * 2, 0, 0);

                var displayScript = newObject.AddComponent<EditorDisplayObject>();
                displayScript.cid = collectionID;
                displayScript.id = objectID;

                if (newObject.tag == "Floor")
                {
                    newObject.transform.localPosition += new Vector3(0, 1, 0); //shift floors up
                }

                //DisableObject(newObject);

                newObject.layer = 11;

                menuItems += 1;
            }
        }

    }

    Vector3 GetRoundedPosition(Vector3 point)
    {
        return new Vector3(Mathf.Round(point.x), 0, Mathf.Round(point.z));
    }

    void DisableObject(GameObject obj)
    {
        MonoBehaviour[] comps = obj.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour c in comps)
        {
            c.enabled = false;
        }
        //newObject.GetComponent<Animator>().enabled = true;
    }

    public void Clear()
    {
        Destroy(levelHolder);

        levelHolder = new GameObject("LevelHolder");
        level = new Level(width, height);
    }

    public void Load()
    {
        string path = Application.dataPath + "/Saves/";

        if (File.Exists(path + level.name + ".json"))
        {
            Clear();

            string str = File.ReadAllText(path + level.name + ".json");

            var sqrObjects = JsonHelper.FromJson<SquareObject>(str);

            foreach (var obj in sqrObjects)
            {
                Square square = new Square(obj.pos);
                square.objects.Add(obj);

                var newObject = prefabManager.collections[obj.cid].objects[obj.id];

                Instantiate(newObject, new Vector3(obj.pos.x, 0, obj.pos.y), new Quaternion(), levelHolder.transform);

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

    void PlaceNewObject()
    {
        if (selectedOriginal == null)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, shootableMask))
        {
            Vector3 playerToMouse = hit.point - transform.position;
            playerToMouse.y = 0f;

            if (level.AddSquareObject(hit.point, selectedInfo.cid, selectedInfo.id, selectedOriginal) != null)
            {
                var newObject = Instantiate(selectedOriginal, GetRoundedPosition(hit.point), new Quaternion(), levelHolder.transform);
                
                newObject.layer = 10;

                var displayScript = newObject.AddComponent<EditorDisplayObject>();
                displayScript.cid = selectedInfo.cid;
                displayScript.id = selectedInfo.id;
                displayScript.pos = GetRoundedPosition(hit.point);
                EraseButton.SetActive(false);
            }
            else
            {
                if (Physics.Raycast(ray, out hit, 100, editorObjectMask))
                {
                    var editorScript = hit.transform.gameObject.GetComponent<EditorDisplayObject>();

                    if (editorScript != null)
                    {
                        stageSelectedObject = hit.transform.gameObject;
                        EraseButton.SetActive(true);
                    }
                }
            }
        }
    }

    public void RemoveObject()
    {        
        //UnselectObject();
        if(stageSelectedObject != null)
        {
            var editorScript = stageSelectedObject.GetComponent<EditorDisplayObject>();
            
            level.RemoveSquareObject(editorScript.pos);
            editorScript.RemoveObject();
            EraseButton.SetActive(false);
        }        
    }

    void UnselectObject()
    {
        if (selectedObject != null)
        {
            selectedObject.transform.localScale = new Vector3(1f, 1f, 1f);
            selectedObject = null;
            selectedOriginal = null;
        }
    }

    void SelectObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, editorMenuObjectMask))
        {
            if (selectedObject == hit.transform.gameObject)
            {

            }
            //activated too many times, need a delay

            UnselectObject();

            selectedObject = hit.transform.gameObject;
            selectedInfo = selectedObject.GetComponent<EditorDisplayObject>();
            selectedOriginal = prefabManager.collections[selectedInfo.cid].objects[selectedInfo.id];
            selectedObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        else
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (clicked == false)
            {
                clicked = true;

                SelectObject();
                PlaceNewObject();
            }
        }
        else
        {
            clicked = false;
        }
    }
}
