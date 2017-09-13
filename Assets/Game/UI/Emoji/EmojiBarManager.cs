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
        Vector2 screenLocation =
            Camera.main.WorldToScreenPoint(
                unit.transform.position);

        var size = unit.GetPixelSize() / 30;

        var bar = Instantiate(emojiPrefab);
        bar.transform.SetParent(canvas.transform, false);
        bar.transform.position = screenLocation;
        
        //text.GetComponentInChildren<Text>().text = damage.ToString();

        bar.transform.localScale = Vector3.Scale(bar.transform.localScale, new Vector3(size, size, size));

        //Destroy(bar, 2);

        var barScript = bar.GetComponent<EmojiBar>();
        barScript.unit = unit;

        return barScript;
    }
}
