using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;

public class LevelEditor : MonoBehaviour
{

    public PrefabManager prefabManager;

    public GameObject defaultSquare;

    public int width = 20;
    public int height = 20;

    EditorDisplayObject selectedInfo;

    Level level;

    GameObject templateLevelHolder;
    GameObject levelHolder;
    GameObject menuObjectHolder;

    public GameObject menuObjectSpawnPoint;
    public GameObject menuObjectTemplate;

    int menuItems = 0;

    int shootableMask;
    int editorMenuObjectMask;
    int editorObjectMask;

    int selectedCollection = -1;

    bool clicked = false;

    public GameObject EraseButton;

    EditorDisplayObject stageSelectedScript;

    // Use this for initialization
    void Start()
    {

        level = new Level();
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



        for (int collectionID = 0; collectionID < prefabManager.collections.Length; collectionID++)
        {

        }

        OnSelectCollection(0);

    }

    public void OnHomeButtonPressed()
    {
        OnSelectCollection(0);
    }

    void OnSelectCollection(int collectionID)
    {
        if(selectedCollection == collectionID)
        {
            return;
        }

        selectedCollection = collectionID;

        if (menuObjectHolder != null)
        {
            Destroy(menuObjectHolder);
        }

        menuObjectHolder = new GameObject("MenuObjectHolder");
        menuObjectHolder.transform.SetParent(menuObjectSpawnPoint.transform, false);

        //Debug.Log("Selected: " + collectionID);
        var collection = prefabManager.collections[collectionID];

        menuItems = 0;

        for (int objectID = 0; objectID < collection.objects.Length; objectID++)
        {
            var original = collection.objects[objectID];

            var newObject = Instantiate(menuObjectTemplate, menuObjectHolder.transform);

            newObject.transform.SetParent(menuObjectHolder.transform, false);
            newObject.GetComponent<Image>().sprite = original.GetComponent<EditorGameObject>().sprite;

            //newObject.transform.localPosition += new Vector3(-menuItems * 2, 0, 0);

            newObject.GetComponent<RectTransform>().localPosition += new Vector3(menuItems * 50, 0, 0);


            var info = newObject.AddComponent<EditorDisplayObject>();
            info.cid = collectionID;
            info.id = objectID;

            if (collectionID == 0)
            {
                int menuSelectID = objectID + 1;
                newObject.GetComponent<Button>().onClick.AddListener(() => OnSelectCollection(menuSelectID));
            }
            else
            {
                newObject.GetComponent<Button>().onClick.AddListener(() => OnPrefabSelect(info));
            }


            if (newObject.tag == "Floor")
            {
                //newObject.transform.localPosition += new Vector3(0, 1, 0); //shift floors up
            }

            //DisableObject(newObject);

            newObject.layer = 11;

            menuItems += 1;

        }
    }

    void OnPrefabSelect(EditorDisplayObject info)
    {
        selectedInfo = info;
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
        level = new Level();
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

                //var newObject = prefabManager.collections[obj.cid].objects[obj.id];
                //Instantiate(newObject, new Vector3(obj.pos.x, obj.pos.y, obj.pos.z), new Quaternion(), levelHolder.transform);

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

    public void Save()
    {
        string str = level.SaveLevel();

        string path = Application.dataPath + "/Saves/";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        File.WriteAllText(path + level.name + ".json", str);
    }

    void CreateNewObject(int cid, int id, IPosition pos)
    {
        var selectedOriginal = prefabManager.collections[cid].objects[id];
        var newObject = Instantiate(selectedOriginal, new Vector3(pos.x, pos.y, pos.z), new Quaternion(), levelHolder.transform);

        newObject.layer = 10;

        var displayScript = newObject.AddComponent<EditorDisplayObject>();
        displayScript.cid = cid;
        displayScript.id = id;
        displayScript.pos = pos;
    }

    void PlaceNewObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (selectedInfo != null)
        {
            if (Physics.Raycast(ray, out hit, 100, shootableMask))
            {
                Vector3 playerToMouse = hit.point - transform.position;
                playerToMouse.y = 0f;

                var selectedOriginal = prefabManager.collections[selectedInfo.cid].objects[selectedInfo.id];

                var spawnPos = (hit.point + prefabManager.collections[selectedInfo.cid].GetComponent<PrefabCollection>().spawnOffset)
                        .ConvertToIPosition();

                if (level.AddSquareObject(spawnPos, selectedInfo.cid, selectedInfo.id, selectedOriginal) != null)
                {
                    CreateNewObject(selectedInfo.cid, 
                                    selectedInfo.id,
                                    spawnPos);
                }
                else
                {

                }
            }
        }

        if (Physics.Raycast(ray, out hit, 100, editorObjectMask))
        {
            var editorScript = hit.transform.gameObject.GetComponent<EditorDisplayObject>();

            if (editorScript != null)
            {
                stageSelectedScript = editorScript;
                EraseButton.SetActive(true);
            }
        }
        else
        {
            //stageSelectedScript = null;
            //EraseButton.SetActive(false);
        }
    }

    public void RemoveObject()
    {
        //UnselectObject();
        Debug.Log("erase");
        if (stageSelectedScript != null)
        {
            level.RemoveSquareObject(stageSelectedScript.pos);
            stageSelectedScript.RemoveObject();
            EraseButton.SetActive(false);
        }
    }

    public float minFov = 15f;
    public float maxFov = 90f;
    public float sensitivity = 10f;
    public float panSensitivity = .01f;

    bool isPanOn = false;

    Vector3 lastPanMousePosition;

    void ZoomFunction()
    {
        float fov = Camera.main.fieldOfView;
        fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
    }

    void PanFunction()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            lastPanMousePosition = Input.mousePosition;
        }

        if (Input.GetButton("Fire1"))
        {
            Vector3 delta = -Input.mousePosition + lastPanMousePosition;
            Camera.main.transform.Translate(delta.x * panSensitivity, delta.y * panSensitivity, 0);
            lastPanMousePosition = Input.mousePosition;
        }

    }

    public void OnPanPressed()
    {
        isPanOn = !isPanOn;
    }

    // Update is called once per frame
    void Update()
    {
        ZoomFunction();

        if (isPanOn)
        {
            PanFunction();
        }
        else
        {
            if (Input.GetButton("Fire1"))
            {
                if (clicked == false)
                {
                    clicked = true;

                    PlaceNewObject();
                }
            }
            else
            {
                clicked = false;
            }
        }
    }
}
