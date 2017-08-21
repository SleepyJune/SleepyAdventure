using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;

public class LevelLoader : MonoBehaviour
{

    public PrefabManager prefabManager;
    
    public GameObject levelSelectionButton;
    public GameObject levelSelectionButtonStart;

    public GameObject levelSelectionMenu;

    GameObject levelSelectionButtonHolder;
        
    GameManager game;

    Level level;

    void Start()
    {        
        level = new Level();

        //Load();

        game = GameObject.Find("GameManager").GetComponent<GameManager>();


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

    public void LoadLevel(string path)
    {
        levelSelectionMenu.SetActive(false);
        game.LoadLevel(path);
    }

}
