using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class GameManager : MonoBehaviour {

    public PrefabManager prefabManager;
    public GameObject playerPrefab;

    GameObject levelHolder;
    Level level;

    // Use this for initialization
    void Start () {
        level = new Level();

        levelHolder = new GameObject("LevelHolder");
    }
	
	// Update is called once per frame
	void Update () {
		
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
            if (level.AddSquareObject(obj.pos, obj.rotation, obj.cid, obj.id, selectedOriginal) != null)
            {
                CreateNewObject(obj.cid, obj.id, obj.pos, obj.rotation);
            }
        }

        InitLevel();

    }

    void InitLevel()
    {
        Pathfinding.InitPathSquares(level);
    }

    void CreateNewObject(int cid, int id, IPosition pos, Vector3 rotation)
    {
        var selectedOriginal = prefabManager.collections[cid].objects[id];

        if (selectedOriginal.tag == "Start")
        {
            selectedOriginal = playerPrefab;
        }

        var newObject = Instantiate(selectedOriginal, new Vector3(pos.x, pos.y / 2.0f, pos.z), Quaternion.Euler(rotation), levelHolder.transform);

    }
}
