using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public PrefabManager prefabManager;
    public GameObject playerPrefab;

    public Unit player;

    public Dictionary<int, Unit> units = new Dictionary<int, Unit>();

    private int unitIDCounter = 0;

    GameObject levelHolder;
    Level level;

    // Use this for initialization
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);

        level = new Level();
        levelHolder = new GameObject("LevelHolder");
    }

    // Update is called once per frame
    void Update()
    {
        BackButton();
    }

    void BackButton()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GetComponent<SceneChanger>().OnLoadButtonPressed("IntroScreen");
        }
    }

    public void LoadLevel(string path)
    {
        level = new Level();

        string str = File.ReadAllText(path);

        var sqrObjects = JsonHelper.FromJson<SquareObject>(str);

        foreach (var obj in sqrObjects)
        {
            Square square = new Square(obj.pos);
            square.objects.Add(obj);

            var selectedOriginal = prefabManager.collections[obj.cid].objects[obj.id];

            var sqrObject = level.AddSquareObject(obj.pos, obj.rotation, obj.cid, obj.id, selectedOriginal);
            if (sqrObject != null)
            {
                var newObject = CreateNewObject(obj.cid, obj.id, obj.pos, obj.rotation);
                sqrObject.SetGameObject(newObject);

                var unitScript = newObject.GetComponent<Unit>();
                if (unitScript != null)
                {
                    CreateUnit(unitScript);
                }

            }
        }

        InitLevel();

    }

    public void CreateUnit(Unit unit)
    {
        units.Add(unitIDCounter, unit);
        unitIDCounter+= 1;
    }

    public void DeleteUnit(Unit unit)
    {
        units.Remove(unitIDCounter);
        Destroy(unit.gameObject);
    }

    void InitLevel()
    {
        Pathfinding.InitPathSquares(level);
    }

    GameObject CreateNewObject(int cid, int id, IPosition pos, Vector3 rotation)
    {
        var selectedOriginal = prefabManager.collections[cid].objects[id];

        if (selectedOriginal.tag == "Start")
        {
            selectedOriginal = playerPrefab;
        }

        var newObject = Instantiate(selectedOriginal, new Vector3(pos.x, pos.y / 2.0f, pos.z), Quaternion.Euler(rotation), levelHolder.transform);

        if (newObject.tag == "Player")
        {
            player = newObject.GetComponent<PlayerMovement>();
        }

        return newObject;
    }

    public PathInfo UnitMoveTo(Unit unit, Vector3 to)
    {
        return UnitMoveTo(unit, unit.transform.position, to);
    }

    public PathInfo UnitMoveTo(Unit unit, Vector3 from, Vector3 to)
    {
        var from2d = from.ConvertToIPosition().To2D();
        var to2d = to.ConvertToIPosition().To2D();

        if (from2d.Distance(to2d) < 2)
        {
            var interactable = level.GetInteractableObject(to2d);

            if (interactable != null)
            {
                if (interactable.Use(unit))
                {
                    return null;
                }
            }
        }

        return Pathfinding.GetShortestPath(from, to);
    }

    public void SetScene(string str)
    {
        SceneManager.LoadScene(str);
    }
}

public static class UnitExtensions
{
    public static PathInfo UnitMoveTo(this Unit unit, Vector3 to)
    {
        return GameManager.instance.UnitMoveTo(unit, to);
    }
}
