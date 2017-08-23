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

    void Start()
    {
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
        string path = Application.dataPath + "/Saves/";

        DirectoryInfo d = new DirectoryInfo(path);

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
