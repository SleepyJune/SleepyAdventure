using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmojiBarManager : MonoBehaviour
{
    public GameObject emojiPrefab;

    GameObject canvas;

    // Use this for initialization
    void Start()
    {
        canvas = GameObject.Find("Canvas/EmojiBarHolder");
    }

    public EmojiBar CreateBar(Monster unit)
    {
        var bar = Instantiate(emojiPrefab);
        bar.transform.SetParent(canvas.transform, false);

        var barScript = bar.GetComponent<EmojiBar>();
        barScript.unit = unit;

        return barScript;
    }
}
