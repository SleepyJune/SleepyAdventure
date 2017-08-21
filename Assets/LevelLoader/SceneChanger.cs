using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene("LevelLoader");
    }

    public void OnEditorButtonPressed()
    {
        SceneManager.LoadScene("LevelEditor");
    }

    public void OnLoadButtonPressed(string str)
    {
        SceneManager.LoadScene(str);
    }
}

