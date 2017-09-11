using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.IO;

public class LevelEditor : MonoBehaviour
{
    [System.NonSerialized]
    public PrefabManager prefabManager;

    public GameObject defaultSquare;
    public GameObject saveScreen;

    public InputField levelNameInput;

    public GameObject levelLoader;

    int width = 100;
    int height = 100;

    EditorDisplayObject selectedInfo;

    Level level;
    
    GameObject levelHolder;

    public GameObject floorPlane;

    public GameObject menuObjectSpawnPoint;
    public GameObject menuObjectTemplate;

    int menuItems = 0;

    int shootableMask;
    int editorMenuObjectMask;
    int editorObjectMask;

    PrefabCollection selectedCollection = null;
    PrefabCollection baseCollection;

    bool clicked = false;

    public GameObject EraseButton;

    EditorDisplayObject stageSelectedScript;

    GameObject indicatorCube;

    Vector3 currentRotation;

    string savePath;

    // Use this for initialization
    void Start()
    {

        level = new Level();
        levelHolder = new GameObject("LevelHolder");
        
        shootableMask = LayerMask.GetMask("Floor");
        editorObjectMask = LayerMask.GetMask("EditorObject");
        editorMenuObjectMask = LayerMask.GetMask("EditorMenuObject");

        floorPlane.transform.localScale = new Vector3(width, 1, height);

        prefabManager = PrefabManager.instance;


        if (Application.platform == RuntimePlatform.Android)
        {
            savePath = Application.persistentDataPath + "/Saves/";
        }
        else
        {
            savePath = Application.dataPath + "/Saves/";
        }

        baseCollection = prefabManager.collections[0];

        OnSelectCollection(baseCollection);
    }

    public void OnSaveScreenButtonPressed()
    {
        levelNameInput.text = level.name;
        saveScreen.SetActive(true);
    }

    public void OnSaveScreenCancelButtonPressed()
    {
        saveScreen.SetActive(false);
    }

    public void OnHomeButtonPressed()
    {
        OnSelectCollection(baseCollection);
    }

    void OnSelectCollection(PrefabCollection collection)
    {
        if (selectedCollection == collection)
        {
            return;
        }

        selectedCollection = collection;

        menuObjectSpawnPoint.transform.DestroyChildren();
        
        //Debug.Log("Selected: " + collectionID);

        menuItems = 0;

        for (int objectID = 0; objectID < collection.objects.Length; objectID++)
        {
            var original = collection.objects[objectID];
            var prefabInfo = original == null ? null : original.GetComponent<PrefabObject>();

            var newObject = Instantiate(menuObjectTemplate, menuObjectSpawnPoint.transform, false);
            newObject.GetComponent<Image>().sprite = prefabInfo.sprite;
            newObject.GetComponentInChildren<Text>().text = prefabInfo.objectName;
            //newObject.GetComponent<Image>().sprite = collection.images[objectID];

            var info = newObject.AddComponent<EditorDisplayObject>();
            info.pid = prefabInfo ? prefabInfo.prefabID : 0;
            info.collection = collection;
                        
            if (collection == baseCollection)
            {
                int menuSelectID = objectID + 1;
                var menuSelectCollection = prefabManager.collections[menuSelectID];

                newObject.GetComponent<Button>().onClick.AddListener(() => OnSelectCollection(menuSelectCollection));
            }
            else
            {
                newObject.GetComponent<Button>().onClick.AddListener(() => OnPrefabSelect(info));
            }

            newObject.layer = 11;

            menuItems += 1;

        }
    }

    void OnPrefabSelect(EditorDisplayObject info)
    {
        selectedInfo = info;
        currentRotation = Vector3.zero;

        //Debug.Log(info.pid);
        //Debug.Log(prefabManager.GetGameObject(info.pid));
    }

    Vector3 GetRoundedPosition(Vector3 point)
    {
        return new Vector3(Mathf.Round(point.x), 0, Mathf.Round(point.z));
    }

    public void Clear()
    {
        Destroy(levelHolder);

        levelHolder = new GameObject("LevelHolder");
        level = new Level();
    }

    public void OnLoadButtonPressed()
    {
        levelLoader.SetActive(true);
    }

    public void LoadLevel(string path)
    {
        //string path = Application.persistentDataPath + "/Saves/";

        //if (File.Exists(path + level.name + ".json"))
        if (File.Exists(path))
        {
            Clear();
                        
            string str = File.ReadAllText(path);

            var sqrObjects = JsonHelper.FromJson<SquareObject>(str);

            foreach (var obj in sqrObjects)
            {
                Square square = new Square(obj.pos);
                square.objects.Add(obj);

                var selectedOriginal = prefabManager.GetGameObject(obj.pid);
                if (level.AddSquareObject(obj.pos, obj.rotation, selectedOriginal) != null)
                {
                    CreateNewObject(obj.pid, obj.pos, obj.rotation);
                }

                //level.map.Add(square.position, square);
            }

            level.name = Path.GetFileNameWithoutExtension(path);
            levelNameInput.text = level.name;

            //level.LoadLevel(str);
        }
    }

    public void Save()
    {
        level.name = levelNameInput.text;

        string str = level.SaveLevel();

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        File.WriteAllText(savePath + level.name + ".json", str);

        saveScreen.SetActive(false);
    }

    public void Save2()
    {
        var savePath = Application.dataPath + "/Resources/Saves/";

        level.name = levelNameInput.text;

        string str = level.SaveLevel();

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        File.WriteAllText(savePath + level.name + ".json", str);        
    }

    void CreateNewObject(int pid, IPosition pos, Vector3 rotation)
    {
        var selectedOriginal = prefabManager.GetGameObject(pid);
        var newObject = Instantiate(selectedOriginal, new Vector3(pos.x, pos.y / 2.0f, pos.z), Quaternion.Euler(rotation), levelHolder.transform);

        newObject.layer = 10;

        var displayScript = newObject.AddComponent<EditorDisplayObject>();
        displayScript.pos = pos;

        DisableComponents(newObject);

        var boxCollider = newObject.AddComponent<BoxCollider>();
    }

    void DisableComponents(GameObject obj)
    {
        MonoBehaviour[] components = obj.GetComponents<MonoBehaviour>();

        foreach (var component in components)
        {
            //Debug.Log(component.GetType().FullName);

            string name = component.GetType().FullName;

            if (name != "EditorGameObject" && name != "EditorDisplayObject")
            {
                component.enabled = false;
            }

        }

        var anim = obj.GetComponent<Animator>();
        if (anim != null)
        {
            anim.enabled = false;
        }

        var colliders = obj.GetComponents<Collider>();
        foreach(var collider in colliders)
        {
            collider.enabled = false;
        }

        var rigidbody = obj.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.isKinematic = true;
        }

    }

    void PlaceNewObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (selectedInfo != null)
        {
            if (Physics.Raycast(ray, out hit, 100, shootableMask))
            {
                var hitPoint = hit.point;
                hitPoint.y = 0;

                var selectedOriginal = prefabManager.GetGameObject(selectedInfo.pid);

                var spawnPos = (hitPoint + new Vector3(0, selectedInfo.collection.height,0))
                        .ConvertToIPosition();

                if (level.AddSquareObject(spawnPos, currentRotation, selectedOriginal) != null)
                {
                    CreateNewObject(selectedInfo.pid,
                                    spawnPos,
                                    currentRotation);
                }
                else
                {

                }
            }
        }
    }

    public void EditorScriptActions()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 100, editorObjectMask))
        {
            var editorScript = hit.transform.gameObject.GetComponent<EditorDisplayObject>();

            if (editorScript != null)
            {
                stageSelectedScript = editorScript;
                //EraseButton.SetActive(true);

                if (Input.GetButtonDown("Fire2"))
                {
                    RemoveObject();
                }
            }
        }
    }

    public void RemoveObject()
    {
        //UnselectObject();
        if (stageSelectedScript != null)
        {
            level.RemoveSquareObject(stageSelectedScript.pos);
            stageSelectedScript.RemoveObject();
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

    void HighlightSquare()
    {
        if (selectedInfo == null)
        {
            return;
        }

        var selectedOriginal = prefabManager.GetGameObject(selectedInfo.pid);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, shootableMask))
        {
            var pos = hit.point.ConvertToIPosition().To2D();

            var hitPoint = hit.point;
            hitPoint.y = 0;

            var spawnPos = (hitPoint + new Vector3(0, selectedInfo.collection.height, 0))
                        .ConvertToIPosition();

            if (indicatorCube == null)
            {
                indicatorCube = Instantiate(selectedOriginal, new Vector3(spawnPos.x, spawnPos.y / 2.0f, spawnPos.z), Quaternion.Euler(currentRotation));
                DisableComponents(indicatorCube);
            }
            else if (indicatorCube.transform.position.ConvertToIPosition().To2D() != pos)
            {
                indicatorCube.transform.position = new Vector3(spawnPos.x, spawnPos.y / 2.0f, spawnPos.z);
            }
        }
        else
        {
            if (indicatorCube != null)
            {
                Destroy(indicatorCube);
            }
        }
    }

    public void OnRotateButtonPress()
    {
        currentRotation += new Vector3(0, 90, 0);
    }

    void BackButton()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GetComponent<SceneChanger>().OnLoadButtonPressed("IntroScreen");
        }
    }

    // Update is called once per frame
    void Update()
    {
        BackButton();
        
        if (Input.GetButtonDown("Fire2"))
        {
            selectedInfo = null;
            Destroy(indicatorCube);
        }

        if (isPanOn)
        {
            PanFunction();
        }
        else
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                HighlightSquare();
                ZoomFunction();

                if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
                {
                    if(indicatorCube == null)
                    {
                        EditorScriptActions();
                    }

                }

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
            else
            {
                if (indicatorCube != null)
                {
                    Destroy(indicatorCube);
                }
            }
        }
    }
}
