﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.IO;

public class LevelEditor : MonoBehaviour
{

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
    GameObject menuObjectHolder;

    public GameObject floorPlane;

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

        //prefabManager = GetComponent<PrefabManager>();


        if (Application.platform == RuntimePlatform.Android)
        {
            savePath = Application.persistentDataPath + "/Saves/";
        }
        else
        {
            savePath = Application.dataPath + "/Saves/";
        }


        for (int collectionID = 0; collectionID < prefabManager.collections.Length; collectionID++)
        {

        }

        OnSelectCollection(0);

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
        OnSelectCollection(0);
    }

    void OnSelectCollection(int collectionID)
    {
        if (selectedCollection == collectionID)
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
            newObject.GetComponent<Image>().sprite = collection.images[objectID];

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
        currentRotation = Vector3.zero;
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

                //var newObject = prefabManager.collections[obj.cid].objects[obj.id];
                //Instantiate(newObject, new Vector3(obj.pos.x, obj.pos.y, obj.pos.z), new Quaternion(), levelHolder.transform);

                var selectedOriginal = prefabManager.collections[obj.cid].objects[obj.id];
                if (level.AddSquareObject(obj.pos, obj.rotation, obj.cid, obj.id, selectedOriginal) != null)
                {
                    CreateNewObject(obj.cid, obj.id, obj.pos, obj.rotation);
                }

                //level.map.Add(square.position, square);
            }


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

    void CreateNewObject(int cid, int id, IPosition pos, Vector3 rotation)
    {
        var selectedOriginal = prefabManager.collections[cid].objects[id];
        var newObject = Instantiate(selectedOriginal, new Vector3(pos.x, pos.y / 2.0f, pos.z), Quaternion.Euler(rotation), levelHolder.transform);

        newObject.layer = 10;

        var displayScript = newObject.AddComponent<EditorDisplayObject>();
        displayScript.cid = cid;
        displayScript.id = id;
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

        var collider = obj.GetComponent<Collider>();
        if (collider != null)
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

                var selectedOriginal = prefabManager.collections[selectedInfo.cid].objects[selectedInfo.id];

                var spawnPos = (hitPoint + new Vector3(0, prefabManager.collections[selectedInfo.cid].height,0))
                        .ConvertToIPosition();

                if (level.AddSquareObject(spawnPos, currentRotation, selectedInfo.cid, selectedInfo.id, selectedOriginal) != null)
                {
                    CreateNewObject(selectedInfo.cid,
                                    selectedInfo.id,
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

        var selectedOriginal = prefabManager.collections[selectedInfo.cid].objects[selectedInfo.id];

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, shootableMask))
        {
            var pos = hit.point.ConvertToIPosition().To2D();

            var hitPoint = hit.point;
            hitPoint.y = 0;

            var spawnPos = (hitPoint + new Vector3(0, prefabManager.collections[selectedInfo.cid].height, 0))
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
        ZoomFunction();

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