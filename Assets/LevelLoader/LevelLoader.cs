using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;

public class LevelLoader : MonoBehaviour
{

    public PrefabManager prefabManager;
    public GameObject playerPrefab;

    public GameObject levelSelectionButton;
    public GameObject levelSelectionButtonStart;

    public GameObject levelSelectionMenu;

    GameObject levelSelectionButtonHolder;

    GameObject levelHolder;

    Level level;

    void Start()
    {
        levelHolder = new GameObject("LevelHolder");
        level = new Level();

        //Load();

        LoadLevelNames();
    }

    void LoadLevelNames()
    {
        levelSelectionButtonHolder = new GameObject("LevelSelectionButtonHolder");
        levelSelectionButtonHolder.AddComponent<RectTransform>();

        levelSelectionButtonHolder.transform.SetParent(levelSelectionButtonStart.transform, false);

        string path = Application.dataPath + "/Saves/";

        DirectoryInfo d = new DirectoryInfo(path);

        int numfiles = 0;
        foreach (var file in d.GetFiles("*.json"))
        {
            var newButton = Instantiate(levelSelectionButton, 
                                        new Vector2(0, -numfiles * 50), 
                                        Quaternion.identity
                                        );

            newButton.transform.SetParent(levelSelectionButtonHolder.transform, false);
            newButton.GetComponentInChildren<Text>().text = Path.GetFileNameWithoutExtension(file.Name);

            string fullPath = file.FullName;

            newButton.GetComponent<Button>().onClick.AddListener(() => LoadLevel(fullPath));

            numfiles += 1;
        }


    }

    void CreateNewObject(int cid, int id, IPosition pos)
    {
        var selectedOriginal = prefabManager.collections[cid].objects[id];

        if (selectedOriginal.tag == "Start")
        {
            selectedOriginal = playerPrefab;
        }

        var newObject = Instantiate(selectedOriginal, new Vector3(pos.x, pos.y / 2.0f, pos.z), new Quaternion(), levelHolder.transform);

    }

    public void LoadLevel(string path)
    {
        levelSelectionMenu.SetActive(false);
        Load(path);
    }

    public void Load(string path)
    {
        string str = File.ReadAllText(path);

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
