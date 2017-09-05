﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public PrefabManager prefabManager;
    public GameObject playerPrefab;

    public Hero player;
    public Transform hud;

    public Dictionary<int, Unit> units = new Dictionary<int, Unit>();
    public Dictionary<int, Projectile> projectiles = new Dictionary<int, Projectile>();
    public Dictionary<int, Obstacle> obstacles = new Dictionary<int, Obstacle>();

    private int unitIDCounter = 0;

    public int gameCounter = 0;

    GameObject levelHolder;
    Level level;

    DamageTextController damageText;

    private bool gameOver = false;

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

        damageText = GetComponent<DamageTextController>();
    }

    // Update is called once per frame
    void Update()
    {
        BackButton();
        DeleteDeadMonsters();
        UpdateWalkableSquares();
        gameCounter += 1;
    }

    void BackButton()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GetComponent<SceneChanger>().OnLoadButtonPressed("IntroScreen");
        }
    }

    void DeleteDeadMonsters()
    {        
        foreach (var key in units.Keys.ToList())
        {
            var unit = units[key];

            if(unit != null && unit is Monster)
            {
                var monster = unit as Monster;

                if(monster.health <= 0)
                {
                    monster.Death();
                }
            }
        }
    }

    void UpdateWalkableSquares()
    {
        /*foreach(var square in level.map.Values)
        {
            square.obstacles = new Dictionary<int, Entity>();
        }*/

        foreach(var unit in units.Values)
        {
            var square = level.GetSquareAtPoint(unit.transform.position.ConvertToIPosition().To2D());
            if (square != null)
            {                
                if(unit.sqr != square)
                {
                    unit.sqr.obstacles.Remove(unit.id);

                    unit.sqr = square;
                    unit.sqr.obstacles.Add(unit.id, unit);

                    unit.pos2d = square.position;
                }

            }
        }
    }

    public bool SameDestination(Unit current, IPosition pos)
    {
        foreach(var unit in units.Values)
        {
            if(current != unit && unit.nextPos == pos)
            {
                return true;
            }
        }

        return false;
    }

    public void LoadLevel(string path)
    {
        level = new Level();

        string str = File.ReadAllText(path);

        var sqrObjects = JsonHelper.FromJson<SquareObject>(str);

        foreach (var obj in sqrObjects)
        {
            IPosition pos2d = obj.pos.To2D();

            Square square = level.GetSquareAtPoint(pos2d);
            
            if(square == null)
            {
                square = new Square(pos2d);
                level.map.Add(square.position, square);
            }
            square.objects.Add(obj);

            var selectedOriginal = prefabManager.collections[obj.cid].objects[obj.id];

            //var sqrObject = level.AddSquareObject(obj.pos, obj.rotation, obj.cid, obj.id, selectedOriginal);
            if (obj != null)
            {
                var newObject = CreateNewObject(obj.cid, obj.id, obj.pos, obj.rotation);
                obj.SetGameObject(newObject);
                                
                var entityScript = newObject.GetComponent<Entity>();
                if (entityScript != null)
                {
                    entityScript.id = unitIDCounter;
                    entityScript.sqr = square;

                    if(entityScript is Obstacle)
                    {
                        entityScript.sqr.obstacles.Add(entityScript.id, entityScript);
                    }

                    entityScript.pos2d = square.position;                    
                }

                var unitScript = newObject.GetComponent<Unit>();
                if (unitScript != null)
                {
                    CreateUnit(unitScript);
                }

                var obstacleScript = newObject.GetComponent<Obstacle>();
                if(obstacleScript != null)
                {
                    CreateObstacle(obstacleScript);
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

    public void DeleteUnit(Unit unit, float time)
    {
        unit.sqr.obstacles.Remove(unit.id);
        unit.collider.isTrigger = true;

        units.Remove(unit.id);

        if (time != 0)
        {
            Destroy(unit.gameObject, time);
        }
        else
        {
            Destroy(unit.gameObject);
        }
    }

    public void CreateObstacle(Obstacle obj)
    {
        obstacles.Add(unitIDCounter, obj);
        unitIDCounter += 1;
    }

    void InitLevel()
    {
        Pathfinding.InitPathSquares(level);
        hud.gameObject.SetActive(true);
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
                    unit.DisableUnitMovement(.5f);
                    return null;
                }
            }
        }

        return Pathfinding.GetShortestPath(unit, from, to);
    }

    public void SetSceneWithWait(string str, float waitTime)
    {
        StartCoroutine(SetScene(str, waitTime));
    }

    private IEnumerator SetScene(string str, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(str);
    }

    public void SetScene(string str)
    {
        SceneManager.LoadScene(str);
    }

    public void CreateDamageText(Unit unit, int damage)
    {
        damageText.CreateDamageText(unit, damage);
    }

    public void CreateProjectile(Unit source, Projectile projectile, Vector3 from, Vector3 to)
    {        
        var proj = Instantiate(projectile, from, Quaternion.identity);

        proj.source = source;
        proj.start = from;
        proj.end = to;

        proj.SetVelocity();       
        
    }

    public void OnPlayerChangeWeapon()
    {
        ((PlayerMovement)player).OnChangeWeapon();
    }

    public void GameOver()
    {
        gameOver = true;
        Camera.main.GetComponent<CameraFollow>().isFollowing = true;
    }
}
