using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;

public class LevelLoader : MonoBehaviour
{
    public GameObject levelSelectionButton;
    Transform levelSelectionGrid;

    delegate void LoadLevelCall(string path);

    LoadLevelCall buttonEvent;

    string savePath;

    void Start()
    {

        if (Application.platform == RuntimePlatform.Android)
        {
            savePath = Application.persistentDataPath + "/Saves/";
        }
        else
        {
            savePath = Application.dataPath + "/Saves/";
        }


        //game = GameObject.Find("GameManager").GetComponent<GameManager>();

        var game = GameObject.Find("GameManager");
        if (game)
        {
            buttonEvent = game.GetComponent<GameManager>().LoadLevel;
        }
        else
        {
            buttonEvent = GameObject.Find("LevelEditor").GetComponent<LevelEditor>().LoadLevel;
        }

        levelSelectionGrid = transform.Find("Panel/List/Grid");

        LoadLevelNames();
    }

    void LoadLevelNames()
    {
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);

            var levels = Resources.LoadAll("Levels", typeof(TextAsset));

            foreach (var obj in levels)
            {
                var level = obj as TextAsset;
                File.WriteAllText(savePath + level.name + ".json", level.text);
            }

        }

        DirectoryInfo d = new DirectoryInfo(savePath);

        int numfiles = 0;
        foreach (var file in d.GetFiles("*.json"))
        {
            var newButton = Instantiate(levelSelectionButton, levelSelectionGrid);

            //newButton.transform.SetParent(levelSelectionButtonHolder.transform, false);
            newButton.GetComponentInChildren<Text>().text = Path.GetFileNameWithoutExtension(file.Name);

            string fullPath = file.FullName;

            newButton.GetComponent<Button>().onClick.AddListener(() => LoadLevel(fullPath));

            numfiles += 1;
        }


    }

    public void LoadLevel(string path)
    {
        buttonEvent(path);
        transform.gameObject.SetActive(false);
    }

}
